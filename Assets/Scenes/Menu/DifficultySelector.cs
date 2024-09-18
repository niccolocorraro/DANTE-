using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;

public class DifficultySelector : MonoBehaviour
{

    public static DifficultySelector instance;
    public DungeonGenerationData easyDifficultyData;
    public DungeonGenerationData mediumDifficultyData;
    public DungeonGenerationData hardDifficultyData;

    private DatabaseReference databaseReference;
    private FirebaseUser currentUser;

     public DungeonGenerationData defaultDiff;

    public DungeonGenerationData dungeonData;
    public UserData userData;  // Reference to UserData instance


    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (currentUser != null)
        {
            LoadUserData(currentUser.UserId);
        }
        else
        {
            Debug.Log("No user is currently authenticated.");
        }
    }

    public void SelectE()
    {
        SetDifficulty(easyDifficultyData, DifficultyLevel.Easy);
    }

    public void SelectM()
    {
        SetDifficulty(mediumDifficultyData, DifficultyLevel.Medium);
    }

    public void SelectH()
    {
        SetDifficulty(hardDifficultyData, DifficultyLevel.Hard);
    }

    private void SetDifficulty(DungeonGenerationData selectedData, DifficultyLevel difficultyLevel)
    {
        if (currentUser != null)
        {
            if (userData == null)
            {
                Debug.Log("UserData is not initialized.");
                return;
            }

            string userId = currentUser.UserId;

            // Aggiorna il livello di difficoltà e i dati del dungeon
            userData.dungeonGenerationData = selectedData;
            userData.difficulty = (int)difficultyLevel;

            // Serializza i dati per Firebase
            string dungeonJson = JsonUtility.ToJson(userData.dungeonGenerationData);
            string difficultyJson = userData.difficulty.ToString();  // Converti la difficoltà in stringa

            // Aggiorna il database Firebase
            databaseReference.Child($"users/{userId}/dungeonGenerationData").SetRawJsonValueAsync(dungeonJson);
            databaseReference.Child($"users/{userId}/difficulty").SetRawJsonValueAsync(difficultyJson);
        }
        else
        {
            Debug.Log("No user is currently authenticated.");
        }
    }

    private void LoadUserData(string userId)
    {
        DatabaseReference userRef = databaseReference.Child($"users/{userId}");

        userRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed to load user data: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                // Carica i dati dei collezionabili, se esistono
                if (snapshot.Child("collectables").Exists)
                {
                    string collectablesJson = snapshot.Child("collectables").GetRawJsonValue();

                    // Modifica: deserializza nel nuovo formato `CollectableData`
                    userData.collectables = JsonUtility.FromJson<CollectableData>(collectablesJson);

                    if (userData.collectables != null && userData.collectables.list != null)
                    {
                        Debug.Log("Loaded collectables data for the user.");
                    }
                    else
                    {
                        Debug.Log("Collectables data does not exist, initializing with default values.");
                        userData.collectables = new CollectableData(); // Inizializza con i dati di default
                    }
                }

                // Carica i dati del dungeon, se esistono
                  if (snapshot.Child("dungeonGenerationData").Exists)
                {
                    string dungeonJson = snapshot.Child("dungeonGenerationData").GetRawJsonValue();
                    Debug.Log(dungeonJson);

                    JsonUtility.FromJsonOverwrite(dungeonJson, dungeonData);
                    userData.dungeonGenerationData = dungeonData;
                    Debug.Log(userData.dungeonGenerationData.ToString());
                    Debug.Log("Loaded dungeonGenerationData for the user.");
                }
                else
                {
                    Debug.Log("dungeonGenerationData does not exist for this user, using default data.");
                }
                // Carica il livello di difficoltà, se esiste
                if (snapshot.Child("difficulty").Exists)
                {
                    userData.difficulty = int.Parse(snapshot.Child("difficulty").Value.ToString());
                }
                else
                {
                    Debug.Log("Difficulty level does not exist for this user, setting to default.");
                    userData.difficulty = 0;  // Default to Easy
                }
            }
            else
            {
                Debug.LogWarning("No data exists for this user in the database.");
                // Inizializza i dati di default
                userData = new UserData
                {
                    collectables = new CollectableData(), // Inizializza collezionabili di default
                    dungeonGenerationData = new DungeonGenerationData(), // Inizializza dati del dungeon
                    difficulty = 0 // Difficoltà di default
                };
            }
        });
    }

    private enum DifficultyLevel
    {
        Easy = 0,
        Medium = 11,
        Hard = 22
    }
}

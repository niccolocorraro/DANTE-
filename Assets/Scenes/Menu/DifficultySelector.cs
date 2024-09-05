using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;

public class DifficultySelector : MonoBehaviour
{
    public DungeonGenerationData easyDifficultyData;
    public DungeonGenerationData mediumDifficultyData;
    public DungeonGenerationData hardDifficultyData;

    private DatabaseReference databaseReference;
    private FirebaseUser currentUser;
    public UserData userData;  // Reference to UserData instance

    public DungeonGenerationData defaultDiff;

    public DungeonGenerationData dungeonData;

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
            Debug.Log(userData.ToString());

            // Ensure userData is initialized
            if (userData == null)
            {
                Debug.Log("UserData is not initialized.");
                return;
            }

            // Ensure collectables inside userData is initialized
            if (userData.collectables == null)
            {
                Debug.Log("Collectables data is not initialized.");
                return;
            }

            string userId = currentUser.UserId;

            // Update the local UserData instance
            userData.dungeonGenerationData = selectedData;

            // Update the difficulty level in userData
            userData.difficulty = (int)difficultyLevel;

            // Debug logs to confirm data is being set
            Debug.Log($"Set difficulty to {difficultyLevel} for user {userId}");

            // Convert the updated UserData fields to JSON
            string dungeonJson = JsonUtility.ToJson(userData.dungeonGenerationData);
            string collectablesJson = JsonUtility.ToJson(userData.collectables);
            string difficultyJson = userData.difficulty.ToString(); // Convert difficulty to string for Firebase


            // Update the database with the new difficulty and collectables
            databaseReference.Child($"users/{userId}/dungeonGenerationData").SetRawJsonValueAsync(dungeonJson);
            databaseReference.Child($"users/{userId}/collectables").SetRawJsonValueAsync(collectablesJson);
            databaseReference.Child($"users/{userId}/difficulty").SetRawJsonValueAsync(difficultyJson); // Update difficulty in Firebase

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
                // Load collectables data if it exists
                if (snapshot.Child("collectables").Exists)
                {
                    string collectablesJson = snapshot.Child("collectables").GetRawJsonValue();
                    Debug.Log(collectablesJson);
                    userData.collectables = JsonUtility.FromJson<CollectablesData>(collectablesJson);   
                    Debug.Log("Loaded collectablesData for the user.");
                }
                else
                {
                    Debug.Log("collectablesData does not exist for this user, initializing with default values.");
                    userData.collectables = new CollectablesData();
                }
                // Load dungeonGenerationData if it exists
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

                // Load difficulty if it exists
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
            }
        });
    }

    private enum DifficultyLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
}

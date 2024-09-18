using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class GeneratoreDungeon : MonoBehaviour
{

    public static GeneratoreDungeon instance;
    public GameObject loadingScreen;
    public Slider progressBar;

    public GameObject portaPrefab;
    public GameObject chiavePrefab;
    public GameObject cantoPrefab;

    public string nomeCanto;

    public CollectableItem collectableSpawned;

    private List<Vector2Int> stanzeDungeon;
    public FirebaseUser currentUser;
    public  DatabaseReference databaseReference;

    public UserData userData;
    public DungeonGenerationData dungeonData;

    void Awake()
    {
        instance = this;
    }

    private async void Start()
    {
        currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        InputManager.instance.BlockInput();  // Block user input at the start
        loadingScreen.SetActive(true);  // Activate the loading screen

        if (currentUser != null)
        {
            await LoadUserData(currentUser.UserId);
        }
        else
        {
            Debug.Log("No user is currently authenticated.");
        }

        await StartDungeonGeneration(userData.dungeonGenerationData);
    }

   private async Task LoadUserData(string userId)
{
    DatabaseReference userRef = databaseReference.Child($"users/{userId}");

    DataSnapshot snapshot = await userRef.GetValueAsync().ContinueWith(task =>
    {
        if (task.IsFaulted)
        {
            Debug.Log("Failed to load user data: " + task.Exception);
            return null;
        }
        return task.Result;
    });

    if (snapshot != null && snapshot.Exists)
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
}

    private async Task StartDungeonGeneration(DungeonGenerationData dungeonGenerationData)
    {
        await GenerateDungeon(dungeonGenerationData);

        // Finalize loading
        progressBar.value = 1f;

        // Wait a moment to show the completed loading bar
        await Task.Delay(500);

        // Hide the loading screen
        loadingScreen.SetActive(false);

        InputManager.instance.UnblockInput();  // Unblock user input after loading is done
    }

    private async Task GenerateDungeon(DungeonGenerationData dungeonGenerationData)
    {
        // Step 1: Generate the dungeon rooms
        stanzeDungeon = DungeonCrawlerController.creaDungeon(dungeonGenerationData);
        await spawnStanze(stanzeDungeon);

        // Step 2: Spawn collectables
        await spawnCollectableItem();

        // Step 3: Handle doors and winning condition
        await attivaVincente();
        await gestisciPorte();
    }

    private async Task spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Prova", 0, 0);

        // Calculate the progress increment per room
        float progressIncrement = 0.4f / stanzeDungeon.Count;

        foreach (Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Prova", roomLocation.x, roomLocation.y);

            // Update progress
            progressBar.value += progressIncrement;

            // Await a frame to ensure the UI updates
            await Task.Yield(); // Non-blocking delay that waits for the next frame
        }
    }

    private async Task spawnCollectableItem()
{
    // Get the correct collectables list based on the selected difficulty
    List<CollectableItem> selectedCollectablesList = userData.collectables.list;

    CollectableItem collectableSpawned = null;

    Debug.Log(userData.collectables.ToString());

    // Iterate over the selected collectables list to find an item that hasn't been found yet
    for (int i = userData.difficulty ; i<= userData.difficulty+9; i++)
    {
        if (!selectedCollectablesList[i].isFound)
        {
            collectableSpawned = selectedCollectablesList[i];
            Debug.Log(selectedCollectablesList[i].name);
            nomeCanto = selectedCollectablesList[i].name;
            break;

        }
    }
    

    if (collectableSpawned != null && stanzeDungeon.Count >= 5)
    {
        // Spawn the collectable in a random room
    
        Instantiate(cantoPrefab, GestoreStanze.instance.stanzeCaricate[Random.Range(4, GestoreStanze.instance.stanzeCaricate.Count() - 3)].GetStanzaCentro(), Quaternion.identity);
    }

    await Task.Yield(); // Wait for the next frame to continue
}


    private async Task gestisciPorte()
    {
        await Task.Delay(1000); // Wait for 1 second
        foreach (Stanza s in GestoreStanze.instance.stanzeCaricate)
        {
            if (s != null)
                s.RimuoviPorte();
        }
        // Update progress
        progressBar.value += 0.3f;
    }

    private async Task attivaVincente()
    {
        await Task.Delay(1000); // Wait for 1 second
        GestoreStanze.instance.stanzeCaricate.Last().isVincente = true;
        spawnKeyAndDoor();
        // Update progress
        progressBar.value += 0.3f;
    }

    private void spawnKeyAndDoor()
    {
        Instantiate(portaPrefab, GestoreStanze.instance.stanzeCaricate.Last().GetStanzaCentro(), Quaternion.identity);
        Instantiate(chiavePrefab, GestoreStanze.instance.stanzeCaricate[Random.Range(4, GestoreStanze.instance.stanzeCaricate.Count() - 2)].GetStanzaCentro(), Quaternion.identity);
    }

    private Vector3 GetRandomPositionInRoom(Stanza room)
    {
        Rect bounds = room.GetRoomBounds();
        float x = Random.Range(bounds.xMin, bounds.xMax);
        float y = Random.Range(bounds.yMin, bounds.yMax);
        return new Vector3(x, y, 0);
    }

    
}

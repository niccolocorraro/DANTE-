using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GeneratoreDungeon : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> stanzeDungeon;

    public GameObject loadingScreen;
    public Slider progressBar;

    private void Start()
    {
        InputManager.instance.BlockInput();  // Block user input at the start
        loadingScreen.SetActive(true);  // Activate the loading screen

        if( GameData.selectedDifficultyData != null)
        {
        dungeonGenerationData = GameData.selectedDifficultyData;
        }

        StartCoroutine(GenerateDungeon());
    }

    private IEnumerator GenerateDungeon()
    {
        // Step 1: Generate the dungeon rooms
        stanzeDungeon = DungeonCrawlerController.creaDungeon(dungeonGenerationData);
        yield return StartCoroutine(spawnStanze(stanzeDungeon));
        
        // Step 2: Handle doors and winning condition
        yield return StartCoroutine(attivaVincente());
        yield return StartCoroutine(gestisciPorte());

        // Step 3: Finalize loading
        progressBar.value = 1f;
        
        // Wait a moment to show the completed loading bar
        yield return new WaitForSeconds(0.5f);

        // Hide the loading screen
        loadingScreen.SetActive(false);

        InputManager.instance.UnblockInput();  // Unblock user input after loading is done
    }

    private IEnumerator gestisciPorte()
    {
        yield return new WaitForSeconds(1f);
        foreach (Stanza s in GestoreStanze.instance.stanzeCaricate)
        {
            if (s != null)
                s.RimuoviPorte();
        }
        // Update progress
        progressBar.value += 0.3f;
    }

    private IEnumerator attivaVincente()
    {
        yield return new WaitForSeconds(1f);
        GestoreStanze.instance.stanzeCaricate.Last().isVincente = true;
        portaGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate.Last().GetStanzaCentro());
        chiaveGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate[Random.Range(4, GestoreStanze.instance.stanzeCaricate.Count() - 2)].GetStanzaCentro());

        // Update progress
        progressBar.value += 0.3f;
    }

    private IEnumerator spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Prova", 0, 0);
        foreach (Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Prova", roomLocation.x, roomLocation.y);
            // Update progress
            progressBar.value += 0.4f / stanzeDungeon.Count;
            yield return null; // Wait for a frame to update UI
        }
    }

    private Vector3 GetRandomPositionInRoom(Stanza room)
    {
        Rect bounds = room.GetRoomBounds();
        float x = Random.Range(bounds.xMin, bounds.xMax);
        float y = Random.Range(bounds.yMin, bounds.yMax);
        return new Vector3(x, y, 0);
    }
}

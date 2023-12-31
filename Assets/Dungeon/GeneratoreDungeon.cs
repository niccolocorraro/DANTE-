using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Usa il namespace Unity Random
using System.Linq;

public class GeneratoreDungeon : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> stanzeDungeon;
  
    

    private void Awake()
    {
        stanzeDungeon = DungeonCrawlerController.creaDungeon(dungeonGenerationData);
        spawnStanze(stanzeDungeon);
        StartCoroutine(attivaVincente());


    }

    IEnumerator attivaVincente()
    {
        yield return new WaitForSeconds(3.0f);
        GestoreStanze.instance.stanzeCaricate.Last().isVincente = true;
        
        chiaveGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate[GestoreStanze.instance.stanzeCaricate.Count - 2].GetStanzaCentro());
         
    }

    private void spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Start", 0, 0);
        foreach (Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Empty", roomLocation.x, roomLocation.y);
        }
    }
}

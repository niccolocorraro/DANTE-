using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratoreDungeon : MonoBehaviour
{

    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> stanzeDungeon;


    private void Start()
    {
        stanzeDungeon = DungeonCrawlerController.creaDungeon(dungeonGenerationData);
        spawnStanze(stanzeDungeon);
    }

    private void spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Start", 0, 0);
        foreach(Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Empty", roomLocation.x, roomLocation.y);
        }
    }

}

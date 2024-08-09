using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        StartCoroutine(gestisciPorte());

    }

    private IEnumerator gestisciPorte(){
        yield return new WaitForSeconds(1f);
        foreach(Stanza s in GestoreStanze.instance.stanzeCaricate)
        if(s!= null)
        s.RimuoviPorte();
    }


    IEnumerator attivaVincente()
    {
        yield return new WaitForSeconds(1f);
        GestoreStanze.instance.stanzeCaricate.Last().isVincente = true;
        portaGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate.Last().GetStanzaCentro());
        chiaveGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate[Random.Range(4,GestoreStanze.instance.stanzeCaricate.Count() -2)].GetStanzaCentro());
    }

    private void spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Prova", 0, 0);
        foreach (Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Prova", roomLocation.x, roomLocation.y);
        }
    }


    private Vector3 GetRandomPositionInRoom(Stanza room)
    {
        Rect bounds = room.GetRoomBounds();
        float x = Random.Range(bounds.xMin , bounds.xMax);
        float y = Random.Range(bounds.yMin , bounds.yMax);
        return new Vector3(x, y, 0);
    }

}


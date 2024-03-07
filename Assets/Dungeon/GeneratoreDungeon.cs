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
        chiaveGen.instance.SpawnObject(GestoreStanze.instance.stanzeCaricate[Random.Range(2,GestoreStanze.instance.stanzeCaricate.Count() -1)].GetStanzaCentro());
         
    }

    private void spawnStanze(IEnumerable<Vector2Int> stanze)
    {
        GestoreStanze.instance.caricaStanza("Prova", 0, 0);
        foreach (Vector2Int roomLocation in stanze)
        {
            GestoreStanze.instance.caricaStanza("Prova", roomLocation.x, roomLocation.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direzioni
{
    up = 0, left = 1,  down = 2, right = 3
};


public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> posizioniVisitate = new List<Vector2Int>();
    private static readonly Dictionary<Direzioni, Vector2Int> direzioniMappa = new Dictionary<Direzioni, Vector2Int>
    {
        {Direzioni.up,Vector2Int.up},
        {Direzioni.left,Vector2Int.left},
        {Direzioni.down,Vector2Int.down},
        {Direzioni.right,Vector2Int.right}
    };

    public static List<Vector2Int> creaDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        for (int i = 0; i < dungeonData.numeroCammini; i++) {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }


        int iterazioni = Random.Range(dungeonData.numeroIterazioniMinime, dungeonData.numeroIterazioniMassime);

        for (int i = 0; i < iterazioni; i++)
        {
            foreach (DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(direzioniMappa);
                posizioniVisitate.Add(newPos);
            }
        }
        return posizioniVisitate;
    }
}

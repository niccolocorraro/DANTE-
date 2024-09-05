
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]


[System.Serializable]   
public class DungeonGenerationData : ScriptableObject
{
    public int numeroCammini;
    public int numeroIterazioniMinime;
    public int numeroIterazioniMassime;


public override string ToString()
    {
        return $"DungeonGenerationData:\n" +
               $"  Numero Cammini: {numeroCammini}\n" +
               $"  Numero Iterazioni Minime: {numeroIterazioniMinime}\n" +
               $"  Numero Iterazioni Massime: {numeroIterazioniMassime}";
    }
}

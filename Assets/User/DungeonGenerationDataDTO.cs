using System;

[System.Serializable]
public class DungeonGenerationDataDTO
{
    public int numeroCammini;
    public int numeroIterazioniMinime;
    public int numeroIterazioniMassime;

    // Constructor for easy instantiation
    public DungeonGenerationDataDTO(int cammini, int minime, int massime)
    {
        numeroCammini = cammini;
        numeroIterazioniMinime = minime;
        numeroIterazioniMassime = massime;
    }
}

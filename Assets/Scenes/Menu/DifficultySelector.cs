using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public DungeonGenerationData  easyDifficultyData;
    public DungeonGenerationData  mediumDifficultyData;
    public DungeonGenerationData  difficultDifficultyData;


    

    public  void SelectE()
    {
        GameData.selectedDifficultyData = easyDifficultyData;
        // Optionally, switch scenes or update UI
    }
    public  void SelectM( )
    {
        GameData.selectedDifficultyData = mediumDifficultyData;
        // Optionally, switch scenes or update UI
    }
    public  void SelectH(  )
    {
        GameData.selectedDifficultyData = difficultDifficultyData;
        // Optionally, switch scenes or update UI
    }
}

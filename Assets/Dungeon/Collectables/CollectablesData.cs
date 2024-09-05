using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class CollectablesData
{
    // A list of lists to hold collectables for different difficulty levels
    public List<List<CollectableItem>> collectablesByDifficulty;

    // Constructor
    public CollectablesData()
    {
        collectablesByDifficulty = new List<List<CollectableItem>>
        {
            new List<CollectableItem>(),  // Easy collectables
            new List<CollectableItem>(),  // Medium collectables
            new List<CollectableItem>(),  // Hard collectables
        };

        for (int i = 1; i <= 11; i++)
        {
            collectablesByDifficulty[0].Add(new CollectableItem { name = "Canto " + i, isFound = false });       // Easy
            collectablesByDifficulty[1].Add(new CollectableItem { name = "Canto " + (i + 11), isFound = false }); // Medium
            collectablesByDifficulty[2].Add(new CollectableItem { name = "Canto " + (i + 22), isFound = false }); // Hard
        }
    }

    // Get the list of collectables based on difficulty index
    public List<CollectableItem> GetCollectablesByDifficulty(int difficulty)
    {
        if (difficulty >= 0 && difficulty < collectablesByDifficulty.Count)
        {
            return collectablesByDifficulty[difficulty];
        }
        else
        {
            Debug.LogError("Invalid difficulty index");
            return null;
        }
    }

   public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        string[] difficultyNames = { "Easy", "Medium", "Hard" };

        for (int i = 0; i < collectablesByDifficulty.Count; i++)
        {
            sb.AppendLine($"{difficultyNames[i]} Collectables:");
            foreach (var item in collectablesByDifficulty[i])
            {
                sb.AppendLine($"- {item.name}: Found = {item.isFound}");
            }
        }

        return sb.ToString();
    }
}

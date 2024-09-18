using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CollectableData 
{
    public List<CollectableItem> list;

        // Constructor
    public CollectableData()
    {
        list = new List<CollectableItem>();

        for (int i = 1; i <= 33; i++)
        {
            list.Add(new CollectableItem { name = "Canto " + i, isFound = false });       // Easy
            
        }
    }

}


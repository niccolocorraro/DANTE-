using UnityEngine;

[System.Serializable]
public class CollectibleItem
{
    public string itemName;
    public GameObject collectiblePrefab;
    public bool isCollected = false;
}

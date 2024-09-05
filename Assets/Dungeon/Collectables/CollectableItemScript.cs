using UnityEngine;

public class CollectableItemScript : MonoBehaviour
{
    public string collectableName;  // Renamed to avoid conflict
    public bool isFound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            GeneratoreDungeon.instance.collectableSpawned.isFound = true;
        }
    }
}

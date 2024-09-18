using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;

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
            MarkCollectableAsFound(GeneratoreDungeon.instance.nomeCanto);
        }
    }

    public void MarkCollectableAsFound(string collectableName)
{
    UserData userData = GeneratoreDungeon.instance.userData;

    DatabaseReference databaseReference = GeneratoreDungeon.instance.databaseReference;

    FirebaseUser currentUser = GeneratoreDungeon.instance.currentUser;


    // Check if the userData and collectables are initialized
    if (userData == null || userData.collectables == null)
    {
        Debug.Log("UserData or collectables data is not initialized.");
        return;
    }

    // Get the list of collectables based on the current difficulty
    List<CollectableItem> selectedCollectablesList = userData.collectables.list;

    // Find the collectable by name and mark it as found
    foreach (var collectable in selectedCollectablesList)
    {
        if (collectable.name == collectableName)
        {
            collectable.isFound = true;
            Debug.Log($"Collectable '{collectableName}' marked as found.");
            break;
        }
    }

    Debug.Log(userData.collectables.ToString());

    

    // Update the collectables in Firebase
    string collectablesJson = JsonUtility.ToJson(userData.collectables);
    databaseReference.Child($"users/{currentUser.UserId}/collectables").SetRawJsonValueAsync(collectablesJson);

    Debug.Log("Updated collectables data in Firebase.");
}

}

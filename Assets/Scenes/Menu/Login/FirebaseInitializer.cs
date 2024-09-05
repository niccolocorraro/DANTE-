using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            Debug.Log("Firebase initialized");
        });
    }
}

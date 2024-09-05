using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System; // For Exception and other System-related classes
using System.Collections;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;
    public CanvasGroup feedbackCanvasGroup;

    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    // Scene to load after successful login or registration
    public string sceneToLoad;

    public DungeonGenerationData defaultDifficultyData;

    public virtual void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void Register()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            ShowFeedback("Username and password cannot be empty.");
            return;
        }

        string email = usernameInput.text + "@example.com";
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Registration failed: " + task.Exception?.GetBaseException().Message);
                ShowFeedback("Registration failed.");
                return;
            }

            Firebase.Auth.AuthResult newUser = task.Result;
            InitializeUserData(newUser.User.UserId, defaultDifficultyData);
            ShowFeedback("Registration successful!");

            // Load the specified scene after successful registration
            LoadNextScene();
        });
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            ShowFeedback("Username and password cannot be empty.");
            return;
        }

        string email = usernameInput.text + "@example.com";
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                ShowFeedback("Login failed: " + task.Exception?.GetBaseException().Message);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            ShowFeedback("Login successful!");

            // Load the specified scene after successful login
            LoadNextScene();
        });
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene specified to load after authentication.");
        }
    }

    private void InitializeUserData(string userId, DungeonGenerationData difficultyData)
    {
        UserData userData = new UserData
        {
            collectables = new CollectablesData(),
            dungeonGenerationData = difficultyData,
        };

        databaseRef.Child("users").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(userData)).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Failed to initialize user data: " + task.Exception?.GetBaseException().Message);
            }
        });

        string dungeonJson = JsonUtility.ToJson(userData.dungeonGenerationData);
        databaseRef.Child($"users/{userId}/dungeonGenerationData").SetRawJsonValueAsync(dungeonJson);

    }

    private void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackCanvasGroup.alpha = 1; // Show feedback
        feedbackCanvasGroup.interactable = true;
        feedbackCanvasGroup.blocksRaycasts = true;
    }

   
}

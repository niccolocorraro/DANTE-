using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    private static Player player;
    public static float health = 3;
    private static int maxHealth = 3;
    private static float moveSpeed = 5f;
    private Animator anim;
    private static Rigidbody2D rb;
    public static bool morto;
    public bool chiaveCheck;
    public bool isWon;

    public bool collectableFound;
    public static event Action OnPlayerDeath;
    public GameObject completeLevelUI;
    public GameObject loadingUI;
    public static float Health { get => health; set => health = value; } 
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            player = FindObjectOfType<Player>(); // Trova l'oggetto Player e assegnalo al riferimento statico
            morto = false;
            health = 3;
            chiaveCheck = false;
        }
    }

    private void Start(){
        anim = GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        

        caricamento();
    }

    // Update is called once per frame
    void Update()
    {

    
       if(isWon){
                StartCoroutine(completeLevel());
       }

       if (GeneratoreDungeon.instance.collectableSpawned != null)
        {
            MarkCollectableAsFound(GeneratoreDungeon.instance.collectableSpawned.name);
        }

       // healthText.text = "Health: " +  health;
         
    }

    private void MarkCollectableAsFound(string collectableName)
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
    List<CollectableItem> selectedCollectablesList = userData.collectables.collectablesByDifficulty[userData.difficulty];

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

    

    // Update the collectables in Firebase
    string collectablesJson = JsonUtility.ToJson(userData.collectables);
    databaseReference.Child($"users/{currentUser.UserId}/collectables").SetRawJsonValueAsync(collectablesJson);

    Debug.Log("Updated collectables data in Firebase.");
}


    public static void DamagePlayer(int damage) {
    
        if(health > 0){
            health -= damage;
            player.anim.SetTrigger("hurtTrigger");
        }
        
        if(health == 0 && !morto){
            KillPlayer();
        
        }   
    }

    public static void HealPlayer(int healAmount) {
        health = Mathf.Min(maxHealth, health + healAmount);

    }

    public static void KillPlayer() {
      player.anim.SetTrigger("deathTrigger");
      rb.bodyType = RigidbodyType2D.Static;
      morto =true;
      OnPlayerDeath?.Invoke();
    }

     public static void riparti(){
        
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public IEnumerator completeLevel(){

       yield return new WaitForSeconds(2f);
       completeLevelUI.SetActive(true);
       rb.bodyType = RigidbodyType2D.Static;
       player.GetComponent<BoxCollider2D>().enabled = false;
    }

    public IEnumerator caricamento(){

       yield return new WaitForSeconds(1f);
       loadingUI.SetActive(true);
    }



}

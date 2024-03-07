using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    public static event Action OnPlayerDeath;
    public GameObject completeLevelUI;
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
       
    }

    // Update is called once per frame
    void Update()
    {

    
       if(isWon){
                StartCoroutine(completeLevel());
       }
       // healthText.text = "Health: " +  health;
         
    }

    

    public static void DamagePlayer(int damage) {
    
        if(health > 0){
            health -= damage;
            AudioManager.instance.PlaySfx("Damage");
            player.anim.SetTrigger("hurtTrigger");
        }
        
        if(health == 0 && !morto){
            AudioManager.instance.PlaySfx("GameOver");
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
        
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public IEnumerator completeLevel(){

       yield return new WaitForSeconds(2f);
       completeLevelUI.SetActive(true);
       rb.bodyType = RigidbodyType2D.Static;
       player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    private static Player player;
    private static float health = 3;
    private static int maxHealth = 3;
    private static float moveSpeed = 5f;
    private Animator anim;
    

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

        }
    }

    private void Start(){
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " +  health;
         if(health<= 0){
            // funziona quasi , non so perche si blocca
            KillPlayer();
        }
    }

    public static void DamagePlayer(int damage) {
        
       
        health -= damage;
        player.anim.SetTrigger("hurtTrigger");
        
     
       
    }

    public static void HealPlayer(int healAmount) {
        health = Mathf.Min(maxHealth, health + healAmount);

    }

    public static void KillPlayer() {
      player.anim.SetTrigger("deathTrigger");

    }


}

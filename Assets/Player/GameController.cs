using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private static float health = 3;
    private static int maxHealth = 3;
    private static float moveSpeed = 5f;
    

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " +  health;
    }

    public static void DamagePlayer(int damage) {
        health -= damage;
        if(Health<=0){                                   //non so perché qua con la maiuscola
            KillPlayer();
        }
    }

    public static void HealPlayer(int healAmount) {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void KillPlayer() {
         
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float startingHealth;
    public float currentHealth{ get; private set; }
    private bool dead;

    private void Start()
    {
        currentHealth = startingHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy")){

            currentHealth = Mathf.Clamp(currentHealth - 1, 0, startingHealth);

            if(currentHealth > 0){
                currentHealth = currentHealth-1;
                anim.SetTrigger("hurtTrigger");
            }
            else{
                Die();
            }
            
        }
    }


    private void Die(){
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("deathTrigger");
    }
   

    private void RestartLevel(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

  
}

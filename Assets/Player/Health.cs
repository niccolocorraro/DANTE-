using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float startingHealth;
    public float currentHealth{ get; private set; }
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

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
                anim.SetTrigger("hurtTrigger");
                 StartCoroutine(Invunerability());
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


       private IEnumerator Invunerability(){
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
  
}

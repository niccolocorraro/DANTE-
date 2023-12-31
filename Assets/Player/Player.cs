using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float speed;
    public Rigidbody2D rigidbody;
    private Vector2 direzioneMossa;
    public Animator anim;
    private SpriteRenderer sprite;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void InputProc()
    {
        float orizzontale = Input.GetAxisRaw("Horizontal");
        float verticale = Input.GetAxisRaw("Vertical");

        direzioneMossa = new Vector2(orizzontale, verticale).normalized;

        UpdateAnimationUpdate();
      
    }

    void Move()
    {

        rigidbody.velocity = new Vector2(direzioneMossa.x * speed, direzioneMossa.y * speed);

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
        InputProc();
        Move();
    }

    private void UpdateAnimationUpdate(){
        
        if(direzioneMossa.x > 0){
            anim.SetBool("running", true);
            sprite.flipX = false;
        }
        else if(direzioneMossa.x < 0){
            anim.SetBool("running", true);
            sprite.flipX = true;
        }
        else{
            anim.SetBool("running", false);
        }
        
    


    }

      private void RestartLevel(){

       GameController.riparti();
    }

}


    

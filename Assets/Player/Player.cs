using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    Rigidbody2D rigidbody;
    private Vector2 direzioneMossa;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void InputProc()
    {
        float orizzontale = Input.GetAxisRaw("Horizontal");
        float verticale = Input.GetAxisRaw("Vertical");

        direzioneMossa = new Vector2(orizzontale, verticale).normalized;

        if(direzioneMossa.x > 0 || direzioneMossa.y > 0 ){
            anim.SetBool("running", true);
        }
        else if(direzioneMossa.x < 0){
            anim.SetBool("running", true);
        }
        else{
            anim.SetBool("running", false);
        }
    }

    void Move()
    {

        rigidbody.velocity = new Vector2(direzioneMossa.x * speed, direzioneMossa.y * speed);

    }

    void FixedUpdate()
    {
        Move();
    }
    // Update is called once per frame
    void Update()
    {

        InputProc();

    }
}


    

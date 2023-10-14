using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    Rigidbody2D rigidbody;
    private Vector2 direzioneMossa;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void InputProc()
    {
        float orizzontale = Input.GetAxisRaw("Horizontal");
        float verticale = Input.GetAxisRaw("Vertical");

        direzioneMossa = new Vector2(orizzontale, verticale).normalized;

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


    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour {
    public float speed;
    public Rigidbody2D rigidbody;
    public Vector2 direzioneMossa;
    public Animator anim;
    private SpriteRenderer sprite;

    private InputM inputs;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        inputs = GetComponent<InputM>();  // Assicurati che il nuovo sistema di input sia usato
    }

    void InputProc()
    {
        // Usa il nuovo sistema di input (InputM)
        direzioneMossa = inputs.MoveInput.normalized;  // Assegna il movimento con il nuovo input system
        UpdateAnimationUpdate();
    }

    void Move()
    {
        rigidbody.velocity = new Vector2(direzioneMossa.x * speed, direzioneMossa.y * speed);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if input is blocked
        if (InputManager.instance.IsInputBlocked())
        {
            rigidbody.velocity = Vector2.zero;  // Stop the player from moving
            return;  // Skip the rest of the update to block input
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
        InputProc();
        Move();
    }

    private void UpdateAnimationUpdate()
    {
        if (direzioneMossa.x > 0)
        {
            anim.SetBool("running", true);
            sprite.flipX = false;
        }
        else if (direzioneMossa.x < 0)
        {
            anim.SetBool("running", true);
            sprite.flipX = true;
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    private void RestartLevel()
    {
        GameController.riparti();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public Animator anim;
    public GameController gameController;

    private void Start(){
        anim = GetComponent<Animator>();
       
    }
    private void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.tag == "Player" && (GameController.instance.chiaveCheck == true)){
            GameController.instance.isWon = true ; 
            anim.SetTrigger("unlock");
        }
    }

}

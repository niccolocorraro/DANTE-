
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
   
    public GameController gameController;


    private void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.tag == "Player" && (GameController.instance.chiaveCheck == true)){
            GameController.instance.isWon = true ; 
        }
    }

}

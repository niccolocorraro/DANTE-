
using UnityEngine;

public class chiave : MonoBehaviour{

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Destroy(gameObject);
            GameController.instance.chiaveCheck = true ; 
        }
    }
}

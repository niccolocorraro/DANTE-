using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScript : MonoBehaviour
{
 
    public GameController gameController;

    void OnTriggerEnter(){
        gameController.completeLevel();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chiaveGen : MonoBehaviour{
    public GameObject objectToSpawn; // Il prefab dell'oggetto da generare
    public static chiaveGen instance;
 

   private void Awake()
    {

     instance = this;
        
    }
    
    public void SpawnObject(Vector3 spawnPosition)
    {
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Destroy(gameObject);
            GameController.instance.chiaveCheck = true ; 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portaGen : MonoBehaviour
{
    
    public GameObject objectToSpawn; // Il prefab dell'oggetto da generare
    public static portaGen instance;
 

   private void Awake()
    {

     instance = this;
        
    }
    
    public void SpawnObject(Vector3 spawnPosition)
    {
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"  ){
            Destroy(gameObject);
           
        }
    }
}

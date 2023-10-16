using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stanza : MonoBehaviour
{

    public int larghezza;
    public int altezza;
    public int x;
    public int y;


    // Start is called before the first frame update
    void Start()
    {
        
        if(GestioneStanze.instance == null)
        {
            Debug.Log("Errore di inizializzazione");
            return;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(larghezza, altezza, 0));
    }

    public Vector3 GetStanzaCentro()
    {

        return new Vector3(x * altezza, y * larghezza);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
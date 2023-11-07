using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stanza : MonoBehaviour
{
    public string nome;
    public int larghezza;
    public int altezza;
    public int X;
    public int Y;
    public bool isVincente = false;


    // Start is called before the first frame update
    void Start()
    {
        
        if(GestoreStanze.instance == null)
        {
            Debug.Log("Errore di inizializzazione");
            return;
        }

        GestoreStanze.instance.SettaStanzaInGrid(this);

    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(larghezza, altezza, 0));
    }

    public Vector3 GetStanzaCentro()
    {

        return new Vector3(X * larghezza, Y * altezza, 0);

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            GestoreStanze.instance.CameraEnterRoom(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

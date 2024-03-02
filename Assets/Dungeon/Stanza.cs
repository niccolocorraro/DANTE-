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

   

    public Porta[] porte;


    // Start is called before the first frame update
    void Start()
    {
        if(GestoreStanze.instance == null)
        {
            Debug.Log("Errore di inizializzazione");
            return;
        }    
        GestoreStanze.instance.SettaStanzaInGrid(this);
        porte = GetComponentsInChildren<Porta>();
    }
     
    public  void RimuoviPorte()
    {
        foreach(Porta p in this.porte)
        {
            switch (p.tipoPorta)
            {
                case Porta.TipoPorta.dx:
                if(getRight())
                   p.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                   else       
                   p.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                 break;
                case Porta.TipoPorta.sx:
                   if(getLeft())
                   p.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                   else       
                   p.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                   break;
                case Porta.TipoPorta.up:
                   if(getAbove())
                   p.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                   else       
                   p.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                   break;
                case Porta.TipoPorta.dw:
                    if(getBottom())
                    p.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    else       
                    p.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    break;
            }
        }
    }

    public bool getRight()
    {
        return GestoreStanze.instance.doesStanzaExist(X + 1, Y);
    }
    public bool getLeft()
    {
       return GestoreStanze.instance.doesStanzaExist(X - 1, Y);
    }
    public bool getAbove()
    {
         return GestoreStanze.instance.doesStanzaExist(X, Y + 1);
    }
    public bool getBottom()
    {
          return GestoreStanze.instance.doesStanzaExist(X , Y - 1);
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

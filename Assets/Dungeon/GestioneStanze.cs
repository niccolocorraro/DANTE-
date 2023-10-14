using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Livello
{
    public string NomeLivello;
    public bool sbloccato;
    public int difficolta;

}


public class infoStanza
{
    public string nome;
    public int x;
    public int y;

}

public class GestioneStanze : MonoBehaviour
{

    public static GestioneStanze instance;

    Livello livello;

    infoStanza infoStanzaCurr;

    Queue<infoStanza> RoomQueueLoader = new Queue<infoStanza>();

    public List<Stanza> stanzeCaricate = new List<Stanza>();



    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }

    public bool doesStanzaExist(int x, int y)
    {
        return stanzeCaricate.Find(item => item.x == x && item.y == y) != null;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

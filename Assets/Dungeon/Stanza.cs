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
    public GameObject enemyPrefab;

    public Rect bounds;

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
        bounds = GetRoomBounds();

        StartCoroutine(SpawnEnemies());
    }

    public void RimuoviPorte()
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
        Gizmos.DrawWireCube(bounds.center, bounds.size);
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

    public Rect GetRoomBounds()
{
    Vector3 center = GetStanzaCentro();
    // Scale factor to reduce the size
    float scaleFactor = 0.75f;
    
    // Calculate scaled dimensions
    float scaledWidth = larghezza * scaleFactor;
    float scaledHeight = altezza * (scaleFactor - 0.15f);
    
    // Return the smaller Rect
    return new Rect(center.x - scaledWidth / 2, (center.y - scaledHeight / 2) + 0.3f, scaledWidth, scaledHeight);
}


    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0f); // Adjust the delay if needed

        
        if (Random.value <= 0.5f && nome !="Inferno Prova(0;0)")
        {
            int enemyCount = Random.Range(1, 3); // Spawn between 1 and 4 enemies
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 randomPosition = GetRandomPositionInRoom();
                GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                NewBehaviourScript enemyScript = enemy.GetComponent<NewBehaviourScript>();
                if (enemyScript != null)
                {
                    enemyScript.room = this;
                    enemyScript.rect = GetRoomBounds();
                }
                
            }
        }
    }   

    private Vector3 GetRandomPositionInRoom()
    {
        Rect bounds = GetRoomBounds();
        float x = Random.Range(bounds.xMin, bounds.xMax);
        float y = Random.Range(bounds.yMin, bounds.yMax);
        return new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

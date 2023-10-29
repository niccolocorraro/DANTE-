using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class infoStanza
{
    public string nome;
    public int X;
    public int Y;
}

public class GestoreStanze : MonoBehaviour
{
    public static GestoreStanze instance;

    string nomeLivelloCorrente = "Inferno";

    infoStanza infoStanzaCorr;

    Stanza stanzaCorrente;

    Queue<infoStanza> RoomQueueLoader = new Queue<infoStanza>();

    public List<Stanza> stanzeCaricate = new List<Stanza>();

    bool isRoomLoaded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        caricaStanza("Start", 0, 0);
        caricaStanza("Empty", 0, 1);
    }

    void Update()
    {
        AggiornaCodaStanze();
    }

    void AggiornaCodaStanze()
    {
        if (isRoomLoaded || RoomQueueLoader.Count == 0)
        {
            return;
        }

        infoStanzaCorr = RoomQueueLoader.Dequeue();
        isRoomLoaded = true;

        StartCoroutine(RoutinCaricamentoStanze(infoStanzaCorr));
    }

    public bool doesStanzaExist(int x, int y)
    {
        return stanzeCaricate.Exists(item => item != null && item.X == x && item.Y == y);
    }

    public void caricaStanza(string nome, int x, int y)
    {
        if (!doesStanzaExist(x, y))
        {
            infoStanza infoNuovaStanza = new infoStanza();
            infoNuovaStanza.nome = nome;
            infoNuovaStanza.X = x;
            infoNuovaStanza.Y = y;
            RoomQueueLoader.Enqueue(infoNuovaStanza);
        }
    }

    IEnumerator RoutinCaricamentoStanze(infoStanza info)
    {
        string nomeStanza = nomeLivelloCorrente + info.nome;

        AsyncOperation caricaStanza = SceneManager.LoadSceneAsync(nomeStanza, LoadSceneMode.Additive);

        while (!caricaStanza.isDone)
        {
            yield return null;
        }

        isRoomLoaded = false;
    }

    public void SettaStanzaInGrid(Stanza stanza)
    {
        stanza.transform.position = new Vector3(
            infoStanzaCorr.X * stanza.larghezza,
            infoStanzaCorr.Y * stanza.altezza,
            0);

        stanza.X = infoStanzaCorr.X;
        stanza.Y = infoStanzaCorr.Y;
        stanza.nome = nomeLivelloCorrente + " " + infoStanzaCorr.nome + "(" + stanza.X + ";" + stanza.Y + ")";
        stanza.transform.parent = transform;

        isRoomLoaded = true;
        if(stanzeCaricate.Count == 0)
        {
            CameraController.instance.stanzaCorrente = stanza;
        }

        stanzeCaricate.Add(stanza);
    }

    public void CameraEnterRoom( Stanza stanza)
    {

        CameraController.instance.stanzaCorrente = stanza;
        stanzaCorrente = stanza;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance; 
    public Stanza stanzaCorrente;
    public float velocitaCamera;


    void Awake()
    {

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        AggiornaPosizione();
        
    }

    void AggiornaPosizione()
    {
        if (stanzaCorrente == null)
        {
            return;
        }

        Vector3 posizioneTarget = getCameraPosizioneTarget();

        transform.position = Vector3.MoveTowards(transform.position, posizioneTarget, Time.deltaTime * velocitaCamera);

    }
    Vector3 getCameraPosizioneTarget()
    {
          if (stanzaCorrente == null)
          {
                return Vector3.zero;
          }


          Vector3 posizoneTarget = stanzaCorrente.GetStanzaCentro();
          posizoneTarget.z = transform.position.z;

        return posizoneTarget;
    }

    public bool isSceneChanged()
    {
        return transform.position.Equals(getCameraPosizioneTarget()) == false; 
    }
}

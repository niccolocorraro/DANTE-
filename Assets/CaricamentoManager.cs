using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaricamentoManager : MonoBehaviour
{

    public GameObject caricamento;

    public void DisableCaricamento(){
         caricamento.SetActive(true);
   }
}

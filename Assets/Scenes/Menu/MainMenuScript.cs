using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
        //se vogliamo passare invece alla prossima scena perché numerate allora scriviamo 
        //SceneManager.GetActiveScene().buildIndex + 1 
        //le scene si numerano nel build settings di unity 
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}

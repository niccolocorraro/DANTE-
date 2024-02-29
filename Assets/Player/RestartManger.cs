using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartManger : MonoBehaviour
{
   public GameObject gameOverMenu;


    public void OnEnable(){
        GameController.OnPlayerDeath += EnableGameOverMenu;
    }

    public void OnDisable(){
        GameController.OnPlayerDeath -= EnableGameOverMenu;
    }
 

   public void EnableGameOverMenu(){
    gameOverMenu.SetActive(true);
   }

   public void RestartLevel(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    GameController.health = 3;
   }

   public void GoToMainMenu(){
    SceneManager.LoadScene("Menu");
   }
}

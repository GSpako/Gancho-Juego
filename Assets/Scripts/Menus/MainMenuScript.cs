using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    // Se cambia el Index en File -> BuildSettings -> Se arrasta escena
    public void PlayGame()
    {
        // DESCOMENTAR ESTA ES LA WENA LA DE ABAJO LA USE PARA PROBAR SI PASA DE NIVEL QUE DA ERROR EESTA AHORA
        //Hola soy Adri�n, lo he encontrado, si da error es porque no est� a�adida la escena en las build settings! 
        //Si da problemas revisar o me envias un mensaje
         SceneManager.LoadScene(GameManager.Instance.levels[0]/*SceneManager.GetActiveScene().buildIndex + 1*/);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.musicPlayedForCurrentLevel = false; // cambiamos de nivel, y ya puede haber nueva musica
    }

    public void QuitGame()
    {
        Debug.Log("Adios :(");
        Application.Quit();
    }

}

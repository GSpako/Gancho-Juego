using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    // Se cambia el Index en File -> BuildSettings -> Se arrasta escena
    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.Instance.levels[0]/*SceneManager.GetActiveScene().buildIndex + 1*/);
        // Empezar a que suene la musica asignada al nivel 1
        PlayerAudioManager.instance.PlayLevelMusic(SceneManager.GetActiveScene().buildIndex + 1, 0.3f);
        GameManager.Instance.musicPlayedForCurrentLevel = false; // cambiamos de nivel, y ya puede haber nueva musica

    }

    public void QuitGame()
    {
        Debug.Log("Adios :(");
        Application.Quit();
    }

}

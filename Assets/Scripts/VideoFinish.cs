using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;



public class VideoFinish : MonoBehaviour
{

    [SerializeField]
    VideoPlayer myVideoPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer.loopPointReached += GoToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            PlayGame();
        }
    }

    void GoToMenu(VideoPlayer vp)
    {
        PlayGame();
    }


    public void PlayGame()
    {
        // DESCOMENTAR ESTA ES LA WENA LA DE ABAJO LA USE PARA PROBAR SI PASA DE NIVEL QUE DA ERROR EESTA AHORA
        //Hola soy Adrián, lo he encontrado, si da error es porque no está añadida la escena en las build settings! 
        //Si da problemas revisar o me envias un mensaje
        SceneManager.LoadScene(GameManager.Instance.levels[0]/*SceneManager.GetActiveScene().buildIndex + 1*/);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.musicPlayedForCurrentLevel = false; // cambiamos de nivel, y ya puede haber nueva musica
    }
}

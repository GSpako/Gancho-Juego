using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado de juego")]
    public GameState gameState;

    [Header("Referencias")]
    public PauseMenuScript pauseMenuScript;
    public GameObject player;
    

    public enum GameState { 
        menu,
        level,
        cinematic
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(Instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuScript.TogglePauseMenu();
           
        }

        if (pauseMenuScript.isGamePaused) 
        {
            Time.timeScale = 0.0f;
            //Debug.Log(Time.timeScale);
            // Se queda el gancho enganchado si pausas el menu :(
            //player.GetComponent<GrappleHook>().enabled = false;
        } else
        {
            Time.timeScale = 1.0f;
            //player.GetComponent<GrappleHook>().enabled = true;
        }

    }

}

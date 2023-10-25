using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager LevelManager;   

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
        gameState = GameState.level;    
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.level)
        {
            pauseMenuScript.TogglePauseMenu();
            gameState = GameState.menu;
        } 
    }

}

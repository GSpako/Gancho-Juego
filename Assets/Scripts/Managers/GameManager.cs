using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager LevelManager;   

    [Header("Estado de juego")]
    public GameState gameState;

    [Header("Referencias")]
    public PauseMenuScript pauseMenuScript;
    public GameObject player;

    [SerializeField] private int currentLevel = 0;
    [SerializeField] private string[] levels;
    

    public enum GameState {
        menu, 
        PauseMenu,
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
            pauseMenuScript.GetComponent<PauseMenuScript>().enabled = true;    
            gameState = GameState.PauseMenu;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.PauseMenu)
        {
            pauseMenuScript.TogglePauseMenu();
            gameState = GameState.level;
        }
    }

    //Se llama desde TimerSystem
    public void EndLevel(bool win/*if player won the game else: menu exit*/) {
        if (win) { 
            //Aquí haremos algo pues el jugador se ha pasado el nivel satisfactoriamente
        }

        StartCoroutine(delay());
    }
    //Cambiar de nivel (actualmente simplemente el siguiente en la lista de niveles)
    public void ChangeLevel() {
        DOTween.KillAll();
        Debug.Log(currentLevel+ " a " +currentLevel+1 % levels.Length);
        SceneManager.LoadScene(levels[++currentLevel % levels.Length]);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(3f);
        ChangeLevel();
    }





}

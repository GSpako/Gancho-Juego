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
    [SerializeField] private string sceneName;
    [SerializeField] public bool musicPlayedForCurrentLevel = false; // Para que solo se reproduzca una vez en el nivel actual



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

        PlayMusic();
    }

    //Se llama desde TimerSystem
    public void EndLevel(bool win/*if player won the game else: menu exit*/) {
        if (win) {
            //Aqu� haremos algo pues el jugador se ha pasado el nivel satisfactoriamente
            PlayerAudioManager.instance.PlayWinLevel();
        }

        StartCoroutine(delay());
    }
    //Cambiar de nivel (actualmente simplemente el siguiente en la lista de niveles)
    public void ChangeLevel() {
        DOTween.KillAll();
       
        currentLevel = (currentLevel + 1) % levels.Length;
        Debug.Log("vamos al nivel " + currentLevel + " de los [0.."+(levels.Length-1)+"]");
        SceneManager.LoadScene(levels[currentLevel]);
        musicPlayedForCurrentLevel = false; // cambiamos de nivel, y ya puede haber nueva musica
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(3f);
        ChangeLevel();
    }

    // Gestionar la musica segun el nivel en el que se esta
    public void PlayMusic()
    {
        if (!musicPlayedForCurrentLevel)
        {
            sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "Level1":
                    PlayerAudioManager.instance.PlayLevelMusic(1, 0.3f);
                    Debug.Log("Musica nivel 1 OK");
                    break;
                case "Level2":
                    PlayerAudioManager.instance.PlayLevelMusic(2, 0.3f);
                    Debug.Log("Musica nivel 2 OK");
                    break;
                case "Level3":
                    PlayerAudioManager.instance.PlayLevelMusic(3, 0.3f);
                    Debug.Log("Musica nivel 3 OK");
                    break;
                case "Movimiento":
                    PlayerAudioManager.instance.PlayLevelMusic(4, 0.3f);
                    Debug.Log("Musica movimiento OK");
                    break;
                case "Menu":
                    PlayerAudioManager.instance.PlayLevelMusic(5, 0.3f);
                    Debug.Log("Musica menu OK");
                    break;
                default:
                    break;
            }

            musicPlayedForCurrentLevel = true; // Ya se reprodujo la musica de este nivel
        }
    }
}

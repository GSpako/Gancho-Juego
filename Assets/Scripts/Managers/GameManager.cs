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
    public string[] levels;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            PlayerAudioManager.instance.PlayWinLevel();
        }

        StartCoroutine(delay());
    }
    //Cambiar de nivel (actualmente simplemente el siguiente en la lista de niveles)
    public void ChangeLevel() {
        DOTween.KillAll();

        currentLevel = (currentLevel + 1) % levels.Length;
        //Debug.Log("vamos al nivel " + currentLevel + " de los [0.."+(levels.Length-1)+"]");
        SceneManager.LoadScene(levels[currentLevel], LoadSceneMode.Single);
        musicPlayedForCurrentLevel = false; // cambiamos de nivel, y ya puede haber nueva musica
        PlayMusic();

    }

    public void SetLevel(int level)
    {
        DOTween.KillAll();

        currentLevel = level;
        //Debug.Log("vamos al nivel " + currentLevel);
        //Debug.Log(levels.Length);
        SceneManager.LoadScene(levels[currentLevel], LoadSceneMode.Single);
        musicPlayedForCurrentLevel = false;
        PlayMusic();

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
            //sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "Level1":
                    PlayerAudioManager.instance.PlayLevelMusic(1, 0.10f);
                    // Asegurarme de que haya un hijo MusicaAudioSource con un AudioSource (pls)
                    Debug.Log("Musica nivel 1 OK");
                    break;
                case "Level2":
                    PlayerAudioManager.instance.PlayLevelMusic(2, 0.10f);
                    Debug.Log("Musica nivel 2 OK");
                    break;
                case "Level3":
                    PlayerAudioManager.instance.PlayLevelMusic(3, 0.10f);
                    Debug.Log("Musica nivel 3 OK");
                    break;
                case "Movimiento":
                    PlayerAudioManager.instance.PlayLevelMusic(4, 0.10f);
                    Debug.Log("Musica movimiento OK");
                    break;
                case "Menu":
                    PlayerAudioManager.instance.PlayLevelMusic(5, 0.10f);
                    Debug.Log("Musica menu OK");
                    break;
                case "LevelAntonio":
                    PlayerAudioManager.instance.PlayLevelMusic(6, 0.10f);
                    Debug.Log("Musica Antonio OK");
                    break;
                case "NivelPako":
                    PlayerAudioManager.instance.PlayLevelMusic(7, 0.10f);
                    Debug.Log("Musica Pako OK");
                    break;
                default:
                    PlayerAudioManager.instance.PlayLevelMusic(1, 0.10f);
                    Debug.Log("Musica NIVEL NO OFICIAL OK");
                    break;
            }

            musicPlayedForCurrentLevel = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualizar el nombre de la escena en el GameManager
        sceneName = scene.name;
        Debug.Log(sceneName);

        musicPlayedForCurrentLevel = false;
        PlayMusic();
    }

}

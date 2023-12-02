using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.SceneManagement;
using static GameManager;

public class PauseMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public PlayerCamera playerCamera;
    public bool isGamePaused = false;
    public static bool isGamePausedStatic;

    [Header("Referencias")]
    [SerializeField] BulletTime bT;
    public GameObject pauseMenuUI;
    public TextMeshProUGUI sensibilidadText;
    public TextMeshProUGUI volumenText;
    public Button continuarButton;
    [SerializeField] GameObject reticula;
    public float sensibilidadXanterior = 850f, sensibilidadYanteriror = 850f;


    private void Awake()
    {
        
    }

    private void Start()
    {
        GameManager.Instance.pauseMenuScript = this;
        if (bT == null) { 
            bT = GameObject.FindObjectOfType<BulletTime>();
        }
    }


    public void VolverAlMenu()
    {
        reticula.SetActive(true);
        bT.bloquearMenus = false;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;

        GameManager.Instance.gameState = GameState.menu;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Sali del juego :(");
        Application.Quit();
    }

    private void Update()
    {
        PauseMenuScript.isGamePausedStatic = isGamePaused;
    }


    public void SetVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
        float volumenNormalizado = Mathf.InverseLerp(-80f, 0f, volumen);
        volumenNormalizado *= 10;
        volumenText.text = volumenNormalizado.ToString("F0");
    }

    public void SetSensibilidad(float sensibilidad)
    {
        if (PlayerCamera.instance != null || PlayerCamera.instance.cameraRespawn)
        {
            PlayerCamera.instance.sensibilityX = sensibilidad;
            PlayerCamera.instance.sensibilityY = sensibilidad * 0.6f;
            sensibilidadText.text = (sensibilidad / 100.0f).ToString("F2");

            // guardar valores de sensibilidad anterior
            sensibilidadXanterior = sensibilidad;
            sensibilidadYanteriror = sensibilidad * 0.6f;
        } 
    }


    public void ContinueGame()
    {
        reticula.SetActive(true);
        bT.bloquearMenus = false;
        //pauseMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        GameManager.Instance.gameState = GameState.level;

        PlayerCamera.instance.cameraRespawn = false;
    }

    public void PauseGame()
    {
        reticula.SetActive(false);

        if (bT == null)
            bT = GameObject.FindObjectOfType<BulletTime>();

        bT.bloquearMenus = true;

        pauseMenuUI.SetActive (true);

        //pauseMenuUI.SetActive(true);
        
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

    }



    public void TogglePauseMenu()
    {
        if (isGamePaused)
        {
            ContinueGame();
        }
        else
        {
            PauseGame();
        }
    }
}

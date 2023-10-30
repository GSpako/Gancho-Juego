using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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


    private void Awake()
    {
        
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
        if (playerCamera != null)
        {
            playerCamera.sensibilityX = sensibilidad;
            sensibilidadText.text = (sensibilidad / 100.0f).ToString("F2");
        }
    }


    public void ContinueGame()
    {
        bT.bloquearMenus = false;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        GameManager.Instance.gameState = GameState.level;
    }

    public void PauseGame()
    {
        bT.bloquearMenus = true;
        pauseMenuUI.SetActive(true);
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

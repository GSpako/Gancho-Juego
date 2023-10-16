using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public PlayerCamera playerCamera;

    private void Awake()
    {
        audioMixer = GetComponent<AudioMixer>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    public void QuitGame()
    {
        Debug.Log("Sali del juego :(");
        Application.Quit();
    }

    public void SetVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
    }

    public void SetSensibilidad(float sensibilidad)
    {
        if(playerCamera != null)
        {
            playerCamera.sensibilityX = sensibilidad;
        }
    }

}

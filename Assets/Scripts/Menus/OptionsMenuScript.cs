using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioClip audioClip;
    public AudioSource audioSource;

    // Nombres claves para configuraciomes
    private const string VolumenKey = "Volumen";


    private void Awake()
    {
        audioSource.clip = audioClip;
    }

    private void Start()
    {
        LoadConfigurations();
    }

    public void SetVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);

        PlayerPrefs.SetFloat(VolumenKey, volumen);
    }

    public void SetFullscreen(bool isfullscreen)
    {
        Screen.fullScreen = isfullscreen;
    }

    public void SetSecreto(bool isSecreto)
    {
        if (isSecreto)
        {
            Debug.Log("AHHHHRGGG");
            audioSource.Play();
        }
    }


    private void LoadConfigurations()
    {
        // Cargar y aplicar volumen
        float volumen = PlayerPrefs.GetFloat(VolumenKey, 0f);
        SetVolumen (volumen);
    }
}
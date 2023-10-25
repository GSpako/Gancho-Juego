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


    private void Awake()
    {
        audioSource.clip = audioClip;
    }

    public void SetVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
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

}
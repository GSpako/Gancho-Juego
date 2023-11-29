using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Quitar el play on Awake, y ponerle Output Master
// PARA SONIDO 3D Poner spacial blend a 1, y cambiar Volume Rolloff a LinearRollOff y cambiar el MinDistance y Max Distance, poner el min cerca del max y au
// un Max distance de 40 va bien creo
[RequireComponent(typeof(AudioSource))]
public class TrapAudioManager : MonoBehaviour
{
    [Header("Sonidos de Trampas")]
    [SerializeField] public AudioClip trapAudioClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float volumen;

    [Tooltip("Si queremos pitch variado poner valor aqui Default= 1.0f")]
    [Range(0.5f, 1.5f)]
    [SerializeField] public float minPitch = 1.0f;
    [Range(0.5f, 1.5f)]
    [SerializeField] public float maxPitch = 1.0f;

    [SerializeField] public AudioSource trapAudioSource;
    private AudioSource currentSoundSource;


    private void Awake()
    {
        if (trapAudioSource == null) 
        {
            trapAudioSource = GetComponent<AudioSource>();
        }
        // Por si aca
        trapAudioClip = trapAudioSource.clip;
    }

    public void PlayTrapSound()
    {
        PlaySound(trapAudioClip, volumen);
    }

    // EN EL START EMPIEZA A SONAR SU RUIDITO
    private void Start()
    {
        PlayTrapSound();
    }

    // Playear un sonido en bucle, que se le pasa, y tocar su volumen
    public void PlaySound(AudioClip sound, float volume)
    {
        if (trapAudioSource != null && sound != null)
        {

            trapAudioSource.clip = sound;
            trapAudioSource.volume = volume;

            float pitch = Random.Range(minPitch, maxPitch);
            trapAudioSource.pitch = pitch;
            trapAudioSource.loop = true;
            trapAudioSource.Play();

            currentSoundSource = trapAudioSource; // Almacena la referencia al AudioSource del sonido actual
        }
    }


    // Parar un sonido de PlaySound() o PlaySoundPitcheado()
    public void StopCurrentSoundClip()
    {
        if (currentSoundSource != null)
        {
            currentSoundSource.Stop();
            currentSoundSource = null; // Limpia la referencia al AudioSource del sonido actual
        }
    }
}

using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Singleton")]
    public static PlayerAudioManager instance;

    [Header("Sonidos del Jugador")]
    public AudioClip jumpSound;
    public AudioClip wallRunSound;
    public AudioClip deathSound;
    public AudioClip hookSound;
    public AudioClip dashSound;
    public AudioClip muelleSound;

    private AudioSource audioSource;        // El sonido que se usara
    private AudioSource currentSoundSource; // Almacena la referencia al AudioSource del sonido actualmente en reproducción

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // PLAY SOUND
    // BASICAMENTE CREAR UNA FUNCION POR SONIDO Y LLAMARLO DONDE SEA CON -> PlayerAudioManager.instance.PlayInserteNombre();

    public void PlayJumpSound()
    {
        PlaySoundPitcheado(jumpSound, 0.4f, 0.8f, 1.2f);
    }

    public void PlayWallRunSound()
    {
        PlaySoundLooped(wallRunSound, 0.5f);
    }


    public void PlayDeathSound()
    {
        PlaySoundPitcheado(deathSound, 0.5f, 0.8f, 1.2f);
    }

    public void PlayDashSound()
    {
        PlaySound(dashSound, 0.5f);
    }

    public void PlayHookSound()
    {
        PlaySoundPitcheado(hookSound, 0.4f, 0.6f, 1f);
    }

    public void PlayMuelleSound()
    {
        PlaySoundPitcheado(muelleSound, 0.5f, 0.8f, 1f);
    }


    public void StopWallRunSound()
    {
        StopWallRunLoopedSound();
    }










    // Playear un sonido, que se le pasa, y tocar su volumen
    public void PlaySound(AudioClip sound, float volume)
    {
        if (audioSource != null && sound != null)
        {
            StopCurrentSoundClip(); // Detiene el sonido actual antes de reproducir uno nuevo

            audioSource.clip = sound;
            audioSource.volume = volume;
            audioSource.Play();

            currentSoundSource = audioSource; // Almacena la referencia al AudioSource del sonido actual
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

    // Playear un sonido, con un pitch entre un rango, para variar el sonido cada vez :O
    // tipo -> PlaySoundPitcheado(deathSound, 1f, 0.8f, 1.2f);
    public void PlaySoundPitcheado(AudioClip sound, float volume, float pitchMin, float pitchMax)
    {
        if (sound != null)
        {
            StopCurrentSoundClip(); // Detiene el sonido actual antes de reproducir uno nuevo

            audioSource.clip = sound;

            float pitch = Random.Range(pitchMin, pitchMax);
            audioSource.pitch = pitch;
            audioSource.volume = volume;

            audioSource.Play();

            currentSoundSource = audioSource; // Almacena la referencia al AudioSource del sonido actual
        }
    }

    // Reproducir un sonido en bucle tipo WallRun o cosas que siempre suenan
    private void PlaySoundLooped(AudioClip sound, float volume)
    {
        if (audioSource != null && sound != null)
        {
            StopCurrentSoundClip(); // Detiene el sonido actual antes de reproducir uno nuevo

            audioSource.clip = sound;
            audioSource.volume = volume;
            audioSource.loop = true; // Activa el modo de bucle
            audioSource.Play();

            currentSoundSource = audioSource; // Almacena la referencia al AudioSource del sonido actual
        }
    }

    // Detener los sonidos de PlaySoundLooped()
    public void StopLoopedSound()
    {
        if (currentSoundSource != null)
        {
            currentSoundSource.loop = false; // Desactiva el modo de bucle
            StopCurrentSoundClip(); // Detiene el sonido actual
        }
    }

    // Detener el sonido en bucle específico de wallRun
    private void StopWallRunLoopedSound()
    {
        if (currentSoundSource != null && currentSoundSource.clip == wallRunSound)
        {
            currentSoundSource.loop = false;
            StopLoopedSound(); // Detener el sonido en bucle
        }
    }
}

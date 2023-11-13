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

    [Header("Sonidos del Jugador")]
    public AudioClip muelleSound;


    private AudioSource audioSource;

   

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

    public void PlayJumpSound()
    {
        PlaySoundPitcheado(jumpSound, 1f, 0.8f, 1.2f);
    }


    public void PlayWallRunSound()
    {
        PlaySound(wallRunSound, 0.6f);
    }

    public void PlayDeathSound()
    {
        PlaySoundPitcheado(deathSound, 1f, 0.8f, 1.2f);
    }

    public void PlayDashSound()
    {
        PlaySound(dashSound, 1f);
    }

    public void PlayHookSound()
    {
        PlaySoundPitcheado(hookSound, 1f, 0.6f, 1f);
    }

    public void PlayMuelleSound()
    {
        PlaySoundPitcheado(muelleSound, 1f, 0.8f, 1f);
    }


    // Playear un sonido, que se le pasa
    private void PlaySound(AudioClip sound, float volume)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.clip = sound;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    // Playear un sonido, con un pitch entre un rango, para variar el sonido cada vez :O
    // tipo -> PlaySoundPitcheado(deathSound, 1f, 0.8f, 1.2f);
    public void PlaySoundPitcheado(AudioClip sound, float volume, float pitchMin, float pitchMax)
    {
        if (sound != null)
        {
            audioSource.clip = sound;

            float pitch = Random.Range(pitchMin, pitchMax);
            audioSource.pitch = pitch;
            audioSource.volume = volume;

            audioSource.Play();
        }
    }
}

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
    public AudioClip winLevelSound;

    [Header("Sonidos de Trampas")]
    public AudioClip laserDisparoSound;


    [Header("Música de Fondo")]
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;
    public AudioClip movimientoMusic;
    public AudioClip menuMusic;
    public AudioClip levelAntonio;
    public AudioClip NivelPako;


    [Header("Audio Sources")]
    public AudioSource audioSource;         // El sonido que se usara
    public AudioSource audioSourceMusic;    // Este playea la musica
    private AudioSource currentSoundSource; // Almacena la referencia al audioSource del sonido actualmente en reproducción


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

        audioSource = GetComponents<AudioSource>()[0];
    }

    // PLAY SOUND
    // BASICAMENTE CREAR UNA FUNCION POR SONIDO Y LLAMARLO DONDE SEA CON -> PlayerAudioManager.instance.PlayInserteNombre();

    public void PlayJumpSound()
    {
        PlaySoundPitcheado(jumpSound, 0.4f, 0.8f, 1.2f);
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
        PlaySoundPitcheado(hookSound, 0.3f, 0.6f, 1f);
    }

    public void PlayMuelleSound() // boing !!!
    {
        PlaySoundPitcheado(muelleSound, 0.5f, 0.8f, 1f);
    }


    public void PlayWallRunSound()
    {
        PlaySoundLooped(wallRunSound, 0.8f);
    }

    // Para el WallRun, que sonaria todo el rato si sigue en la pared, y si cae se acaba :(
    public void StopWallRunSound()
    {
        StopWallRunLoopedSound();
    }

    public void PlayWinLevel()
    {
        PlaySound(winLevelSound, 0.6f);
    }

    public void PlayLaserShootSound()
    {
        PlaySoundPitcheado(laserDisparoSound, 0.1f, 0.6f, 1.1f);
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



    // MUSICA UGHHHH
    // MUSICA OHHHHH


    // LLamar al PlayLevelMusic(1, 2, 3.....) segun el nivel y como lo tengamos puesto
    // Esto lo puse en GameManager, no se funciona aun xd
    public void PlayLevelMusic(int level, float volumen)
    {
        StopCurrentLevelMusic(); // Se para la musica antes de empezar otra
        AudioClip levelMusic = GetLevelMusic(level); // Con la funcion GetLevelMusic() para ver que nivel

        if (levelMusic != null && audioSourceMusic != null)
        {
            MusicClipManager.instance.SaveCurrentMusicClip(levelMusic);
            audioSourceMusic.clip = levelMusic;
            audioSourceMusic.loop = true;
            audioSourceMusic.volume = volumen;
            audioSourceMusic.Play();
        }
    }

    // Detiene la música de fondo del nivel actual
    public void StopCurrentLevelMusic()
    {
        if (audioSourceMusic != null)
        {
            audioSourceMusic.loop = false;
            audioSourceMusic.Stop();
        }
    }

    public void AsegurarMusicAudioSource(int level, float volumen)
    {
            //Debug.Log("AsegurarMusicAudioSource");
            audioSourceMusic.clip = GetLevelMusic(level);
    }

    // Obtiene el clip de música correspondiente al nivel
    private AudioClip GetLevelMusic(int level)
    {
        // Intenta obtener el clip de MusicClipManager
        AudioClip savedClip = MusicClipManager.instance.GetCurrentMusicClip();

        if (savedClip != null)
        {
            return savedClip;
        }
        else
        {
            // Añadir mas casos al switch segun los niveles
            switch (level)
            {
                case 1:
                    return level1Music;
                case 2:
                    return level2Music;
                case 3:
                    return level3Music;
                case 4:
                    return movimientoMusic;
                case 5:
                    return menuMusic;
                case 6:
                    return levelAntonio;
                case 7:
                    return NivelPako;
                default:
                    return null;
            }
        }
    }
}

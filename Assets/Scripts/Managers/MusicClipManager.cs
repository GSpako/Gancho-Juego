using UnityEngine;

public class MusicClipManager : MonoBehaviour
{
    // Singleton
    public static MusicClipManager instance;

    // Almacena el clip de m�sica actual
    [SerializeField] public static AudioClip currentMusicClip;

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
    }

    // Guarda el clip de m�sica actual
    public void SaveCurrentMusicClip(AudioClip clip)
    {
        currentMusicClip = clip;
    }

    // Obtiene el clip de m�sica actual
    public AudioClip GetCurrentMusicClip()
    {
        return currentMusicClip;
    }
}

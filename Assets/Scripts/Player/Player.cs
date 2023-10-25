using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Singleton")]
    public static Player instance;

    [Header("References")]
    public static PlayerCamera camera;

    [Header("Parameters")]
    public LayerMask deathLayer;
    public AudioSource audioSource;

    private bool isDying;

    // Start is called before the first frame update
    private void Awake()
    {
        
        if (instance == null) {
            Debug.LogWarning("Nuevo Jugador");
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
            Debug.LogWarning("Jugador anterior ha sido sobreescrito por " + this);
        }
    }
    private void Start()
    {
        if (TimerSystem.instance != null)
            TimerSystem.instance.StartTimer();

        if (camera == null)
        {
            if (PlayerCamera.instance != null)
                camera = PlayerCamera.instance;
            else
                camera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        }

        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public void kill() {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        Spawner.instance.Spawn(Spawner.types.player);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerCamera.instance.doTilt(new float[]{-20,20}[Random.Range(0,2)]);
        PlayerCamera.instance.GetComponent<Camera>().backgroundColor = Color.red;
        GetComponent<PlayerMovement>().enabled = false;
        camera.enabled = false;
        Destroy(gameObject,Spawner.instance.respawnTime*0.9f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !isDying)
        {
            kill(); isDying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit") && TimerSystem.instance != null)
            TimerSystem.instance.ExitLevel();
    }
}

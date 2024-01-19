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
    public GrappleHook hookSphere;

    [Header("Parameters")]
    public LayerMask deathLayer;

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
        CanvasBehaviour.instance.Log("Empezamos");

        if (TimerSystem.instance != null)
            TimerSystem.instance.StartTimer();

        if (camera == null)
        {
            if (PlayerCamera.instance != null)
                camera = PlayerCamera.instance;
            else
                camera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        }

    }

    public void kill()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Dashing>().enabled = false;
        GetComponent<GrappleHook>().enabled = false;
        if (!isDying)
        {
            isDying = true;

            PlayerAudioManager.instance.PlayDeathSound();

            GameManager.Instance.LevelManager.spawner.Spawn(Spawner.types.player);
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            PlayerCamera.instance.doTilt(new float[] { -20, 20 }[Random.Range(0, 2)]);
            PlayerCamera.instance.GetComponent<Camera>().backgroundColor = Color.red;
            //GetComponent<PlayerMovement>().enabled = false;
            //camera.enabled = false;

            //DashSlider.instance.StopAllCoroutines();
            DashSlider.instance.StopDashCooldown();
            DashSlider.instance.sliderObject.SetActive(false);
            

            hookSphere.stopGrapple();

            
            
            Destroy(gameObject, GameManager.Instance.LevelManager.spawner.respawnTime * 0.9f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((deathLayer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer && !isDying)
        {
            kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit") && TimerSystem.instance != null) {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.enabled = false;
            TimerSystem.instance.ExitLevel();
        }
    }
}

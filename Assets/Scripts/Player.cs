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
        if (camera == null)
        {
            if (PlayerCamera.instance != null)
                camera = PlayerCamera.instance;
            else
                camera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        }
    }

    public void kill() {
        Spawner.instance.Spawn(Spawner.types.player);
        GetComponent<PlayerMovement>().enabled = false;
        Destroy(gameObject,Spawner.instance.respawnTime*0.99f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) { kill(); }
    }
}

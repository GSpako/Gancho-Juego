using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [Header("Prefabs")]
    public GameObject player;

    [Header("Parameters")]
    public float respawnTime;

    Camera notPlayerCamera;

    public enum types { 
        player
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else { 
            Destroy(this);
            Debug.LogWarning(this + " ha sido borrado pues ya hab�a una instancia de Spawner");
        }
    }

    private void Start()
    {
        notPlayerCamera = GetComponentInChildren<Camera>(); 
    }

    public void Spawn(types t) {

        if (instance == null)

        Debug.LogWarning("REVIVIDO?");
        switch (t) {
            case types.player:
                Invoke("rezPlayer", respawnTime); 
                break;
        }
    }

    private void rezPlayer() {
        Instantiate(player,transform.position,transform.rotation);
    }

    private void LateUpdate()
    {
        if (Player.instance != null && notPlayerCamera.enabled)
            notPlayerCamera.enabled = false;    
        else if (Player.instance == null && !notPlayerCamera.enabled)  
            notPlayerCamera.enabled = true;
    }
}

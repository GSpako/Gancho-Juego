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

    public enum types { 
        player
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else { 
            Destroy(this);
            Debug.LogWarning(this + " ha sido borrado pues ya había una instancia de Spawner");
        }
    }

    public void Spawn(types t) {
        switch (t) {
            case types.player:
                Invoke("rezPlayer", respawnTime); 
                break;
        }
    }

    private void rezPlayer() {
        Instantiate(player,transform.position,transform.rotation);
    }

    private void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player;

    [Header("Parameters")]
    public float respawnTime;
    public float sensiX, sensiY;

    Camera notPlayerCamera;

    public enum types { 
        player
    }

    private void Start()
    {
        notPlayerCamera = GetComponentInChildren<Camera>(); 
    }

    public void Spawn(types t) {
        switch (t) {
            case types.player:
                Invoke("rezPlayer", respawnTime);
                CambiarColorSala.instance.RestaurarColoresOriginales();
                
                break;
        }
    }

    private void rezPlayer() {
        Instantiate(player,transform.position,transform.rotation);
        // Para el bug de que la camara no guardaba la sensisibilidad al respawnear
        PlayerCamera.instance.cameraRespawn = true;
        sensiX = GameManager.Instance.pauseMenuScript.sensibilidadXanterior;
        sensiY = GameManager.Instance.pauseMenuScript.sensibilidadYanteriror;
        PlayerCamera.instance.sensibilityX = sensiX;
        PlayerCamera.instance.sensibilityY = sensiY;

    }

    private void LateUpdate()
    {
        if (Player.instance != null && notPlayerCamera.enabled)
            notPlayerCamera.enabled = false;    
        else if (Player.instance == null && !notPlayerCamera.enabled)  
            notPlayerCamera.enabled = true;
    }
}

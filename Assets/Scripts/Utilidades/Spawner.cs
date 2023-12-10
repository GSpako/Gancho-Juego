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
    [SerializeField] private float sensiX = 850f;
    [SerializeField] private float sensiY = 850f; // Fix al bug de que sino cambias sensibilidad no se mueve la camara xd

    Camera notPlayerCamera;

    public enum types { 
        player
    }

    private void Start()
    {
        notPlayerCamera = GetComponentInChildren<Camera>();
        sensiX = GameManager.Instance.pauseMenuScript.sensibilidadXanterior;
        sensiY = GameManager.Instance.pauseMenuScript.sensibilidadYanteriror;
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
        DashSlider.instance.playerMovement = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerMovement>();

    }

    private void LateUpdate()
    {
        if (Player.instance != null && notPlayerCamera.enabled)
            notPlayerCamera.enabled = false;    
        else if (Player.instance == null && !notPlayerCamera.enabled)  
            notPlayerCamera.enabled = true;
    }
}

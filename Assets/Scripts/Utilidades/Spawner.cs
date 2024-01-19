using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player;

    [Header("Parameters")]
    public float respawnTime;
    private bool playerDead; // Cuando se muere y aun no pulsa nada

    [SerializeField] private float sensiX = 850f;
    [SerializeField] private float sensiY = 850f;

    public GameObject panelMuerte;

    Camera notPlayerCamera;

    public enum types
    {
        player
    }

    private void Start()
    {
        notPlayerCamera = GetComponentInChildren<Camera>();
        sensiX = GameManager.Instance.pauseMenuScript.sensibilidadXanterior;
        sensiY = GameManager.Instance.pauseMenuScript.sensibilidadYanteriror;
        panelMuerte = CanvasReferences.instance.panelMuerte;
        panelMuerte.SetActive(false);
        playerDead = false;
    }

    public void Spawn(types t)
    {
        switch (t)
        {
            case types.player:
                StartCoroutine(RespawnPlayer());
                //CambiarColorSala.instance.RestaurarColoresOriginales();
                break;
        }
    }

    IEnumerator RespawnPlayer()
    {
        panelMuerte.SetActive(true); // Que se ve el panel de muerte
        Time.timeScale = 0f; 
        playerDead = true;

        while (!Input.anyKeyDown) // Hasta que no pulse tecla nanai
        {
            yield return null; 
        }

        Instantiate(player, transform.position, transform.rotation);

        // Restablece la sensibilidad de la cámara
        PlayerCamera.instance.cameraRespawn = true;
        PlayerCamera.instance.sensibilityX = sensiX;
        PlayerCamera.instance.sensibilityY = sensiY;
        DashSlider.instance.playerMovement = Player.instance.gameObject.GetComponent<PlayerMovement>();

        panelMuerte.SetActive(false); // Ocultar el panel
        Time.timeScale = 1f; // Reanudar el juego
        playerDead = false; // Uso pergamino de revivir :O
    }

    private void LateUpdate()
    {
        if (Player.instance != null && notPlayerCamera.enabled)
            notPlayerCamera.enabled = false;
        else if (Player.instance == null && !notPlayerCamera.enabled)
            notPlayerCamera.enabled = true;
    }
}

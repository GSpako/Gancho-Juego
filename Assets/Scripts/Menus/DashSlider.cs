using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DashSlider : MonoBehaviour
{
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private Slider dashSlider;
    [SerializeField] public GameObject sliderObject;
    public PlayerMovement playerMovement;
    public bool bloquearMenus = false;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        // Asignar la referencia del Slider y GameObject en el Inspector de Unity
        dashSlider = CanvasReferences.instance.dash;
        sliderObject = dashSlider.gameObject;

        dashSlider.minValue = 0f;
        dashSlider.maxValue = 1f;
        dashSlider.value = 1f; // Inicia con el Dash disponible
    }

    void Update()
    {
        if (playerMovement.recharge)
        {
            // Reducir el valor del slider durante el Dash
            dashSlider.value -= 1f / dashCooldown * Time.deltaTime;

            // Si el Dash ha terminado, marcarlo como no activo y reiniciar el valor del slider
            if (dashSlider.value <= 0f)
            {
                dashSlider.value = 1f;
            }
        } else if (playerMovement.grounded || playerMovement.isWallRunning) 
        {
            dashSlider.value = 1f;

        }

    }

    public void StartDashCooldown()
    {
        // Iniciar el cooldown del Dash
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        // Esperar el tiempo de enfriamiento y luego restablecer el valor del slider
        yield return new WaitForSeconds(dashCooldown);
        dashSlider.value = 1f; // Restablecer el Dash disponible
    }

    void FixedUpdate()
    {
        // Si la barra est� al m�ximo, ocultarla; en caso contrario, mostrarla
        sliderObject.SetActive(dashSlider.value < dashSlider.maxValue * 0.99f);
    }
}

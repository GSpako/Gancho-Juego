using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientation;
    public Transform playerCamera;
    private Rigidbody rb;
    private PlayerMovement pm;
    private float startNumberOfDashes;
    [Tooltip("Añadir en inspecto")]
    public PlayerCamera playerCameraScript;

    [Header("Dashing variables")]
    public float dashForce = 10f;
    public float dashUpwardForce;
    public float dashDuration = 0.25f;
    public float maxDashYSpeed;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = true;
    public bool resetVelocity = true;
    public float cameraDashFov = 100f;
    private float cameraStartFov = 80f;

    [Header("Cooldown")]
    public float dashCooldown = 1f;
    private float dashCooldownTimer;
    private Vector3 delayedForceToApply;

    [Header("Inputs")]
    public KeyCode dashKey = KeyCode.LeftShift;

    Vector3 oldVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        startNumberOfDashes = pm.numberOfDashes;
    }

    private void Update()
    {
        if(Input.GetKeyDown(dashKey))
        {
            StartDash();
        }

        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void StartDash()
    {

        if(dashCooldownTimer > 0)
        {
            return; // si puede devuelve la funcion
        }
        else
        {
            // empezar el timer
            dashCooldownTimer = dashCooldown;
        }


        pm.isDashing = true;
        pm.numberOfDashes--;
        pm.maxYSpeed = maxDashYSpeed;
        playerCameraScript.DoFov(cameraDashFov);

        Transform forwardT;

        // si usar el Forward de la camara o el del orientation (jugador)
        if(useCameraForward)
        {
            forwardT = playerCamera;
        } else 
        { 
            forwardT = orientation; 
        }

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if(disableGravity)
        {
            rb.useGravity = false;
        }

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    // A veces la fuerza se añade antes que se le aplique la del dash
    private void DelayedDashForce()
    {
        if(resetVelocity)
        {
            oldVelocity = rb.velocity;
            rb.velocity = Vector3.zero;
        }

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    private void ResetDash()
    {
        rb.velocity = oldVelocity;
        pm.isDashing = false;
        pm.maxYSpeed = 0;
        pm.numberOfDashes = startNumberOfDashes;
        playerCameraScript.DoFov(cameraStartFov);
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        // 8 direcciones
        if(allowAllDirections)
        {
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        }
        else
        {
            direction = forwardT.forward;
        }

        // si no se mueve (solo en el suelo si eso)
        if(verticalInput == 0 && horizontalInput == 0)
        {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }
}

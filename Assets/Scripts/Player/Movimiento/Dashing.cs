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
    private float startcurrentDashes;
    [Tooltip("A?adir en inspecto")]
    public PlayerCamera playerCameraScript;

    [Header("Dashing variables")]
    public float dashForce = 10f;
    public float dashUpwardForce;
    public float dashDuration = 0.25f;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        startcurrentDashes = pm.currentDashes;
    }

    private void Update()
    {
        if(Input.GetKeyDown(dashKey) && pm.currentDashes > 0 && !pm.grounded && !pm.isWallRunning && !pm.isSliding)
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
        Transform forwardT;

        if (dashCooldownTimer > 0)
        {
            return;
        }

        pm.lastDashM = Time.time;
        pm.recharge = true;
        // empezar el timer
        dashCooldownTimer = dashCooldown;
        
        pm.isDashing = true;
        pm.currentDashes--;
        playerCameraScript.DoFov(cameraDashFov);
        //Efecto de sonido de dash
        PlayerAudioManager.instance.PlayDashSound();

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

    // A veces la fuerza se a?ade antes que se le aplique la del dash
    private void DelayedDashForce()
    {
        if(resetVelocity)
        {
            rb.velocity = Vector3.zero;
        }
        rb.velocity = rb.velocity / 2;
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    private void ResetDash()
    {
        pm.isDashing = false;
        pm.maxYSpeed = 0;
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

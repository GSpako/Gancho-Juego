using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;


    [Header("Variables")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    private Vector3 grapplePoint;
    public bool isGrappling;

    [Header("Cooldown")]
    public float grapplingCooldown;
    private float grapplingCooldownTimer;

    [Header("Inputs")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode grappleImpulseKey = KeyCode.Mouse2;



    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        if(Input.GetKey(grappleKey))
        {
            StartGrapple();
        }

        if(grapplingCooldownTimer > 0)
        {
            grapplingCooldownTimer -= Time.deltaTime;
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void LateUpdate()
    {
        if(isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    // Tiene 2 fases, freeze donde espera un poco y luego 
    // Execute que es ya atraer al player al punto de enganche
    private void StartGrapple()
    {
        // aun no esta activo, esta en cooldown
        if(grapplingCooldownTimer > 0)
        {
            return;
        }

        isGrappling = true;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        } else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {

    }

    private void StopGrapple()
    {
        isGrappling = false;

        grapplingCooldownTimer = grapplingCooldown;

        lr.enabled = false;
    }
}

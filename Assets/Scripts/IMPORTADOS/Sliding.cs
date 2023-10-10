using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientation;
    public Transform playerObj;
    public Rigidbody rb;

    public PlayerMovement playerMovement;

    [Header("Sliding variables")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    public float downForce = 5f;

    [Header("KeyCodes")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private bool isSlidingVar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(Input.GetKeyUp(slideKey) && isSlidingVar)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(isSlidingVar == true)
        {
            SlidingMovement();
        }
    }


    private void StartSlide()
    {
        isSlidingVar = true;

        playerObj.localScale  = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);

        rb.AddForce(Vector3.down * downForce, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void StopSlide()
    {
        isSlidingVar = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if(slideTimer < 0)
        {
            StopSlide();
        }
    }
}

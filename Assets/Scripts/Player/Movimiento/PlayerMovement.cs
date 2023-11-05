using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementSpeed = 7f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float groundDrag = 2f;
    public Transform playerObj;

    [Header("Dash")]
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    public float maxDashes;
    public float currentDashes;
    [Tooltip("Se inicializa en Dashing.cs")]
    public float maxYSpeed;

    public bool isDashing;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump;

    [Header("Agacharse")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    private float startYScale;
    private float crouchForce = 5f;

    [Header("KeyCodes")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public bool grounded;
    public float extraRayDistance = 0.2f;

    [Header("Rampa")]
    public float maxSlopeAngle = 60f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Sliding variables")]
    private bool cieling;
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public float slideYScale;
    private float slideStartYScale;
    public bool isSliding;
    public float downForce = 5f;
    public float slideSpeed;
    public float speedIncreaseMultiplier = 1.5f;
    public float slopeIncreaseMultiplier = 2.5f;

    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public float wallRunForce = 200f;
    public float wallRunSpeed = 8.5f;
    public float wallCheckDistance = 0.5f;
    public float wallRunDelay = .25f;
    private float wallRunExitTime;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallRight;
    private bool wallLeft;
    public bool isWallRunning;

    [Header("Wall Jump")]
    public float wallJumpUpForce = 10;
    public float wallJumpSideForce = 10;
    public float maxWallRunTimer = 3f;
    private Vector3 wallJumpDir;

    [Header("Camara efectos")]
    public PlayerCamera playerCamera;
    public float cameraStartFov = 80f;
    public float cameraFov = 90f;
    public float cameraSlideFov = 100f;
    public float cameraSprintFov = 90f;
    public float cameraTilt = 5f;

    private bool upwardsRunning;
    private bool downwardsRunning;
    private float wallClimbSpeed = 3f;

    // variables para el momentum
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("UI")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI movStateText;

    [Space]
    public Transform orientation;
    public Transform cameraPosition;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;


    private MovementState lastState;
    private bool keepMomentum;

    public MovementState movState;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
        sliding,
        wallrunning,
        dashing
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
        slideStartYScale = transform.localScale.y;

        isSliding = false;

        speedText = CanvasReferences.instance.speedText;
        movStateText = CanvasReferences.instance.movStateText;

        if (playerCamera == null) {
            playerCamera = Player.camera;
        }
    }
    private void Update()
    {
        // checkear si esta en el suelo :O
        float dist = playerHeight * 0.5f + extraRayDistance;
        grounded = Physics.Raycast(transform.position, Vector3.down, dist, whatIsGround);
        cieling = Physics.Raycast(transform.position, Vector3.up, dist, whatIsGround);

        MyInput();
        CheckForWall();
        SpeedControl();
        MovementStateHandler();

        // aplicarle drag si esta en el suelo
        if (movState == MovementState.walking || movState == MovementState.sprinting || movState == MovementState.crouching)
        {
            rb.drag = groundDrag;
        }
        else if (grounded) {
            rb.drag = 0f;
        }
        else
        {
            rb.drag = 0;
        }

        if (Input.GetKeyDown(slideKey)) //&& (horizontalInput != 0 || verticalInput != 0)) //&& !isSliding)
        {
            StartSlide();            
        }
        if ((Input.GetKeyUp(slideKey) || !Input.GetKey(slideKey)) && isSliding)
        {
            if (!cieling)
            {
                StopSlide();
            }
        }
        if (grounded || isWallRunning)
        {
            currentDashes = maxDashes;
        }

        //Debug.Log(rb.velocity + " " + OnSlope().ToString());

    }
    void FixedUpdate()
    {
        MovePlayer();
        //ChangeUi();

        if (isSliding || cieling)
        {
            SlidingMovement();
        }

        if(isWallRunning) {
            WallRunningMovement();
        }
    }


    private void ChangeUi()
    {
        speedText.text = "Speed: " + rb.velocity.magnitude.ToString("F2");
        movStateText.text = movState.ToString();
    }

    /**
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 50, 200, 40), "Speed: " + rb.velocity.magnitude.ToString("F2"));
        GUI.Label(new Rect(20, 70, 200, 40), "MovState: " + movState.ToString());
    }*/


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);


        if(Input.GetKey(jumpKey) && readyToJump && grounded) 
        {
            readyToJump = false;
            PlayerJump();

            // para poder saltar manteniendo el Espacio
            Invoke(nameof(ResetPlayerJump), jumpCooldown);
        }

        // empezar a agacharse
        if(Input.GetKeyDown(crouchKey) && grounded && rb.velocity.magnitude <= walkSpeed  && !GetComponent<GrappleHook>().grapling)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        if(Input.GetKeyUp(crouchKey) || (!grounded && movState == MovementState.crouching))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        // Wallrunning - 1 (Si toca y no esta en el suelo)
        if ((wallLeft || wallRight) && verticalInput > 0 && !grounded && Time.time > wallRunExitTime + wallRunDelay)
        {
            if (!isWallRunning)
            {
                StartWallRun();
            }
            if (Input.GetKey(jumpKey))
            {
                StopWallRun();
                WallJump();
            }
        }
        else
        {
            StopWallRun();
        }

    }

    private void MovePlayer()
    {
        // por temas de gravedas y rampas, fix algo xd
        if(movState == MovementState.dashing) { return; }

        // calcular direccion de movimiento
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope && grounded)
        {
            if (movState != MovementState.sliding)
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * movementSpeed * 20f, ForceMode.Force);
            
            // Para que no este pegado a la rampa
            else 
            {
                rb.AddForce(Vector3.down * 300f);
            }
        }
        // en el suelo
        else if (grounded && movState != MovementState.sliding)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)  // si esta en el aire * airMultiplier
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        // Para que no caiga sino se mueve, se le quita la gravedad al rigibody
        if (!isWallRunning)
        {
            rb.useGravity = !OnSlope();
        }
    }


    private void SpeedControl()
    {

        if (OnSlope() && !exitingSlope)
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limitar la velocidad si es necesario y tal
            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            }
        }
    }

    private void PlayerJump()
    {
        exitingSlope = true;

        // asegurar que la y es 0, para siempre saltar igual
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void ResetPlayerJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }



    private void MovementStateHandler()
    { 
        // Si en la pared 
        if(isWallRunning)
        {
            movState = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        // si separados, Run y Crouch a la vez
        else if (Input.GetKey(crouchKey) && rb.velocity.magnitude <= walkSpeed && grounded && !OnSlope() && !GetComponent<GrappleHook>().grapling)
        {
            movState = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        } 
        // Sprinting
        else if (grounded && Input.GetKey(sprintKey) && !GetComponent<GrappleHook>().grapling)
        {
            movState = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;

            playerCamera.DoFov(cameraSprintFov);
        }
        // Sliding else if (isSliding) 
        else if (Input.GetKey(slideKey) && (slideTimer > 0 && movState != MovementState.crouching || GetComponent<GrappleHook>().grapling))
        {
            movState = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            } else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        // Walking 
        else if (grounded)
        {
            movState = MovementState.walking;
            desiredMoveSpeed = walkSpeed;

            playerCamera.DoFov(cameraStartFov);
        }
        // Air
        else 
        {
            movState = MovementState.air;

            if(desiredMoveSpeed < sprintSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            } else
            {
                desiredMoveSpeed = sprintSpeed;
            }

            // Si esta en el aire puede dashear
            if (isDashing && currentDashes != 0)
            {
                movState = MovementState.dashing;
                desiredMoveSpeed = dashSpeed;
                speedChangeFactor = dashSpeedChangeFactor;
            }
        }

        // Por si cambia mucho, 4f seria la diferencia, y ahi deberia ser lento el cambio
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && movementSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        } 
        else
        {
            movementSpeed = desiredMoveSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if(lastState == MovementState.dashing)
        {
            keepMomentum = true;
        }

        // Si queremos momentum en tipo de movimiento, dentro del if, sino fuera
        if(desiredMoveSpeedHasChanged)
        {   
            if(keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            } else
            {
                StopAllCoroutines();
                movementSpeed = desiredMoveSpeed;
            }
        }

        // Guardar los anteriores
        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = movState;
    }

    // Para que la velocidad se vaya interpolando, y poder acumular velocidad (conservar momentum)
    private float speedChangeFactor; // dash
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - movementSpeed);
        float startValue = movementSpeed;

        float boostFactor = speedChangeFactor; // dash 

        while (time < difference)
        {
            movementSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            } else {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            time += Time.deltaTime * boostFactor; // dash
            
            yield return null;
        }

        movementSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f; // dash
        keepMomentum = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            // angulo de la rampa, sabiendolo con el Raycast
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        // si no golpea nada
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        // aqui se calcula el angulo sobre el que esta la Rampa y el jugador, para aplicarle fuerza en la direccion de la rampa
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    // SLIDING

    private void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);

        playerCamera.DoFov(cameraSlideFov);

        slideTimer = maxSlideTime;
    }

    private void StopSlide()
    {
        isSliding = false;
        playerCamera.DoFov(cameraStartFov);

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideStartYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    // ############################################
    // ############  WALL RUNNING  ################
    // ############################################

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private void StartWallRun()
    {
        wallCheckDistance = 3f;

        //Empezar wallrun
        isWallRunning = true;

        //Quitar la gravedad y el movimiento vertical
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.y, 0f, rb.velocity.z);

        // Efectos de Fov de camara
        playerCamera.DoFov(cameraFov);

        // Efecto inclinacion de la camara
        if (wallLeft)
        {
            playerCamera.doTilt(-cameraTilt);
            wallJumpDir = orientation.right;
        }
        else if (wallRight)
        {
            playerCamera.doTilt(cameraTilt);
            wallJumpDir = -orientation.right;
        }
    }

    private void StopWallRun()
    {
        wallCheckDistance = 1f;

        //Terminar wallrun y reactivar gravedad
        isWallRunning = false;
        rb.useGravity = true;

        //Quitar efectos de camara
        playerCamera.DoFov(cameraStartFov);
        playerCamera.doTilt(0f);

    }

    private void WallRunningMovement()
    {
        // Para ir en el forward de la pared siempre, sera Vector entre Arriba y Normal de la pared = wallForward
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // Sin esto solo funciona por una direccion y por el otro lado salo disparado de la pared xd
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //Moverse hacia adelante
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //Atraer al muro
        rb.AddForce(-wallNormal * 5, ForceMode.Force);
    }

    private void WallJump()
    {
        wallRunExitTime = Time.time;
        rb.velocity = rb.velocity*3 / 4;
       
        rb.AddForce(wallJumpDir * wallJumpSideForce, ForceMode.Force);
        //impulso vertical del wallJump
        rb.velocity = new Vector3(rb.velocity.x, wallJumpUpForce, rb.velocity.z);
    }

}

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
    public float slideForce;
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
    public float muroAtraccion;
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

    [Header("Referencias")]
    [SerializeField] private GrappleHook gp;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI movStateText;

    [Space]
    public Transform orientation;
    public Transform cameraPosition;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    private bool keepMomentum;

    private MovementState lastState;
    public MovementState moveState;
    public enum MovementState
    {
        dashing,
        jumping,
        sliding,
        wallrunning,
        sprinting,
        crouching,
        mov
    }

    private void StateHandler()
    {
        lastState = moveState;

        if (Input.GetKey(jumpKey) && grounded)
        {
            moveState = MovementState.jumping;
        }
        else if (Input.GetKey(slideKey) && rb.velocity.magnitude > walkSpeed)
        {
            moveState = MovementState.sliding;
        }
        else if ((wallLeft || wallRight) && verticalInput > 0 && !grounded && Time.time > wallRunExitTime + wallRunDelay)
        {
            moveState = MovementState.wallrunning;
        }
        else if (Input.GetKey(slideKey))
        {
            moveState = MovementState.crouching;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            moveState = MovementState.sprinting;
        }
        else
        {
            moveState = MovementState.mov;
        }

    }

    private void movement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);
        switch (moveState)
        {
            case MovementState.dashing:
                break;

            case MovementState.jumping:
                Jump();
                break;

            case MovementState.sliding:
                if (lastState != MovementState.sliding) { StartSlide(); }
                SlidingMovement();
                break;

            case MovementState.wallrunning:
                break;

            case MovementState.sprinting:
                rb.AddForce(moveDirection.normalized * sprintSpeed, ForceMode.Force);
                break;

            case MovementState.crouching:
                break;

            case MovementState.mov:
                if (!grounded && gp.grapling) // si esta en el aire con el gancho
                {
                    rb.AddForce(moveDirection.normalized * walkSpeed * 10f * airMultiplier, ForceMode.Force);
                }
                else
                { //Caso base
                    rb.AddForce(moveDirection.normalized * walkSpeed, ForceMode.Force);
                }
                break;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void Start()
    {
        if (playerCamera == null) { playerCamera = Player.camera; }

        gp = GetComponent<GrappleHook>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        isSliding = false;

        startYScale = transform.localScale.y;
        slideStartYScale = transform.localScale.y;

        speedText = CanvasReferences.instance.speedText;
        movStateText = CanvasReferences.instance.movStateText;
    }

    private void Update()
    {
        DoRaycasts(); // hacer raycasts para el suelo, techo, y paredes
        StateHandler();
        Debug.Log(moveState);
        MyInput();
        movement();
        //SpeedControl();

        // aplicarle drag si esta en el suelo
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        MyInput();
        movement();
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            // angulo de la rampa, sabiendolo con el Raycast
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }
        // si no golpea nada
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)// aqui se calcula el angulo sobre el que esta la Rampa y el jugador, para aplicarle fuerza en la direccion de la rampa
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    // ############################################
    // ############### MISCELANEO #################
    // ############################################

    void DoRaycasts()
    {
        float dist = playerHeight * 0.5f + extraRayDistance;
        cieling = Physics.Raycast(transform.position, Vector3.up, dist, whatIsGround);
        grounded = Physics.Raycast(transform.position, Vector3.down, dist, whatIsGround);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    //Este metodo cambia la hitbox del personaje al agacharse
    private void ChangeTransform()
    {
        if (Input.GetKeyDown(crouchKey) && grounded && rb.velocity.magnitude <= walkSpeed && !GetComponent<GrappleHook>().grapling)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        if (Input.GetKeyUp(crouchKey) || (!grounded && moveState == MovementState.crouching))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void resetDash() //RESETEAR DASHES SI APROPIADO
    {
        if (grounded || isWallRunning)
        {
            currentDashes = maxDashes;
        }
    }

    // ############################################
    // #############      JUMP     ################
    // ############################################

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // ############################################
    // ############     SLIDING    ################
    // ############################################

    private void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }

    private void StopSlide()
    {
        isSliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideStartYScale, playerObj.localScale.z);
    }

    // ############################################
    // ############  WALL RUNNING  ################
    // ############################################

    private void StartWallRun()
    {
        wallCheckDistance = 3f;

        //Empezar wallrun
        isWallRunning = true;

        //Quitar la gravedad y el movimiento vertical
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.y, 0f, rb.velocity.z);

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
        rb.AddForce(-wallNormal * muroAtraccion, ForceMode.Force);
    }

    private void WallJump()
    {
        wallRunExitTime = Time.time;
        rb.velocity = rb.velocity * 3 / 4;

        rb.AddForce(wallJumpDir * wallJumpSideForce, ForceMode.Force);
        //impulso vertical del wallJump
        rb.velocity = new Vector3(rb.velocity.x, wallJumpUpForce, rb.velocity.z);
    }

    // ############################################
    // QUARENTENA DE CODIGO

    /*
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
    }*/

    /*
    // Para que la velocidad se vaya interpolando, y poder acumular velocidad (conservar momentum)
    private float speedChangeFactor; // dash
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - movementSpeed);
        float startValue = movementSpeed;

        float boostFactor = speedChangeFactor; // dash 

        //Revisar esto por el posible lag?

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
    }*/
    /*
    private void ChangeUi()
    {
        speedText.text = "Speed: " + rb.velocity.magnitude.ToString("F2");
        movStateText.text = moveState.ToString();
    }
    */
    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 50, 200, 40), "Speed: " + rb.velocity.magnitude.ToString("F2"));
        GUI.Label(new Rect(20, 70, 200, 40), "moveState: " + moveState.ToString());
    }*/
}
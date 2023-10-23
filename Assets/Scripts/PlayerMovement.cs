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
    public float dashSpeed = 10f;
    public float dashSpeedChangeFactor = 50f;
    public float numberOfDashes = 1f;
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
    bool grounded;
    public float extraRayDistance = 0.2f;

    [Header("Rampa")]
    public float maxSlopeAngle = 60f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Sliding variables")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public float slideYScale;
    private float slideStartYScale;
    private bool isSliding;
    public float downForce = 5f;
    public float slideSpeed;
    public float speedIncreaseMultiplier = 1.5f;
    public float slopeIncreaseMultiplier = 2.5f;

    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public float wallRunForce = 200f;
    public float wallRunSpeed = 8.5f;
    public float wallCheckDistance = 0.5f;
    public float wallRunExitTime;
    public float wallRunDelay = .25f;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallRight;
    private bool wallLeft;
    public bool isWallRunning;

    [Header("Wall Jump")]
    [Range(0f, 1f)]
    public float wallJumpUpForce = 0.65f;
    [Range(0f, 1f)]
    public float wallJumpSideForce = 0.6f;
    public float exitWallTime = 0.2f;
    private bool exitingWall;
    private float exitWallTimer;
    public float maxWallRunTimer = 3f;
    private float wallRunTimer;

    [Header("Camara efectos")]
    public PlayerCamera playerCamera;
    public float cameraStartFov = 80f;
    public float cameraFov = 90f;
    public float cameraSlideFov = 100f;
    public float cameraSprintFov = 90f;
    public float cameraTilt = 5f;



    /**
    [Header("Gravedad")]
    public bool useGravity;
    public float gravityCounterForce;
    */

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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraRayDistance, whatIsGround);


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

        if (Input.GetKeyUp(slideKey) && isSliding)
        {
            StopSlide();
        }

        //Debug.Log(rb.velocity + " " + OnSlope().ToString());

    }
    void FixedUpdate()
    {
        MovePlayer();
        ChangeUi();

        if (isSliding == true)
        {
            SlidingMovement();
        }

        if(isWallRunning == true) {
            WallRunningMovement();
        }
    }


    private void ChangeUi()
    {
        speedText.text = "Speed: " + movementSpeed.ToString("F2");
        movStateText.text = movState.ToString();
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);


        if(Input.GetKey(jumpKey) && readyToJump && grounded) 
        {
            //Debug.Log("Espacio");
            readyToJump = false;
            PlayerJump();

            // para poder saltar manteniendo el Espacio
            Invoke(nameof(ResetPlayerJump), jumpCooldown);
        }

        // empezar a agacharse
        if(Input.GetKeyDown(crouchKey) && grounded && rb.velocity.magnitude <= walkSpeed  && !GetComponent<GrappleHook>().grapling)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            
            // Para acercar al player al suelo y que no este flotando, al cambiarle la escala
            //rb.AddForce(Vector3.down * crouchForce, ForceMode.Force);
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
            // Sin esto el personaje va mas rapido en la rampa
            if(rb.velocity.magnitude > movementSpeed)
            {
                //Nota de adri�n esto capea la velocidad m�xima //TODO Podemos reponerlo en el futuro si da problemas quitarlo

                //rb.velocity = rb.velocity.normalized * movementSpeed;
                
            } 
            else
            {
                Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                // limitar la velocidad si es necesario y tal
                if (flatVelocity.magnitude > movementSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                    //rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);

                }

                // limitar velocidad horizontal
                //float maxAirSpeed = movementSpeed * 20f;
                //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxAirSpeed, maxAirSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxAirSpeed, maxAirSpeed));
            }

            // Para no dashear mirando hacia arriba tanto
            if(maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            {
                //rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
            }
        }
    }


   


    private void PlayerJump()
    {
        exitingSlope = true;

        // asegurar que la y es 0, para siempre saltar igual
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        //rb.AddForce(transfofrm.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetPlayerJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private MovementState lastState;
    private bool keepMomentum;


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
            //Debug.Log("Sprinting");
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

            //Debug.Log("Walking");
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
            if (isDashing && numberOfDashes != 0)
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
        } else
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
        //rb.AddForce(Vector3.down * downForce, ForceMode.Force);

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

        // Si no esta sliding en rampa normalmente o en el aire
        if (!OnSlope() || rb.velocity.y > -0.1f)
        {
            //rb.AddForce(inputDirection.normalized, ForceMode.Force);
            //slideTimer -= Time.deltaTime;
        } else
        {
            //rb.AddForce(GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            // aqui no timer, asi slideas siempre en rampas
        }

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
        Debug.DrawRay(transform.position, orientation.right, Color.green);
        Debug.DrawRay(transform.position, -orientation.right, Color.red);
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
            playerCamera.doTilt(-cameraTilt);
        else if (wallRight)
            playerCamera.doTilt(cameraTilt);
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

        rb.AddForce(playerCamera.transform.forward * 200, ForceMode.Force);
        rb.velocity = new Vector3(rb.velocity.x, 5f, rb.velocity.z);

        //Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        //Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        //    wallForward = -wallForward;

        //rb.AddForce(wallForward * 12, ForceMode.Force);
    }
    // WALL RUNNING

    /*
    private void CheckForWall()
    {
       wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
       wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }


    private void StartWallRun()
    {
        isWallRunning = true;
        wallRunTimer = maxWallRunTimer;
        rb.velocity = new Vector3(rb.velocity.y, 0f, rb.velocity.z);

        // Efectos de la camara
        playerCamera.DoFov(cameraFov);
        if(wallLeft)
        {
            playerCamera.doTilt(-cameraTilt);
        } 
        else if(wallRight)
        {
            playerCamera.doTilt(cameraTilt);
        }
    }


    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;

        playerCamera.DoFov(cameraStartFov);
        playerCamera.doTilt(0f);

    }

    private void WallRunningMovement()
    {
        rb.useGravity = false; // no gravedad que si no se cae el man

        // Para ir en el forward de la pared siempre, sera Vector entre Arriba y Normal de la pared = wallForward
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        
        // Sin esto solo funciona por una direccion y por el otro lado salo disparado de la pared xd
        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) 
        {
            wallForward = -wallForward;
        }

        // A�adir fuerza en la direccion de la pared
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        


        // Esta parte es para que suba paredes en diagonal
        if(upwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }

        if (downwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }

        // Para pegarlo a la pared bien, segun si Pulsa A o D
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)) 
        {
            rb.AddForce(-wallNormal * 50, ForceMode.Force);
        }
 
        /**
        // Hacer mas floja la gravedad
        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
        
    }*/
    /*
    private void WallJump()
    {
        
        exitingWall = true;
        exitWallTimer = exitWallTime;
        

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToAplly = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.useGravity = true;
        rb.AddForce(forceToAplly, ForceMode.Impulse);
    }*/

}

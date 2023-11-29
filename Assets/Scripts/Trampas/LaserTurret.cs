using System.Collections;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [Header("Referencias")]
    private Player player;
    public Transform boca; // El punto de origen del láser

    [Header("Variables Torreta")]
    public float detectionRadius = 10f;
    public float rotationSpeed = 5f;
    public float shootingCooldown = 4f;
    [Header("Variables Laser")]
    public float laserSpeed = 2f;
    public float laserMaxLength = 20f;
    public float playerPositionDelay = 1f;
    [SerializeField] private float startWidth = 1f;
    [SerializeField] private float endWidth = 1f;
    [SerializeField] private float emissionBrightness = 10f;
    public LayerMask playerLayer; 
    private bool playerDetected = false;
    private bool canShoot = true;
    private LineRenderer laserLine;


    [Header("Aturdimiento")]
    public float turretStunTime = 3f;
    public ParticleSystem stunParticles;
    public AudioSource stunSound;
    public AudioSource laserSound;
    public AudioSource recoverStunSound;
    public static bool isStunned = false;

    private Vector3 lastPlayerPosition; // Nueva variable para almacenar la última posición del jugador

    private void Start()
    {
        laserLine = gameObject.AddComponent<LineRenderer>();
        ConfigureLineRenderer(laserLine, Color.red);
        stunSound = gameObject.AddComponent<AudioSource>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        StartCoroutine(ScanForPlayer());
    }

    private void Update()
    {
        StunTurret();

        if (playerDetected)
        {
            RotateTowardsPlayer();
        }


    }

    private void ConfigureLineRenderer(LineRenderer lineRenderer, Color color)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.material.EnableKeyword("_EMISSION");
        lineRenderer.material.SetColor("_EmissionColor", Color.red * emissionBrightness); 
        lineRenderer.material.color = color;
        lineRenderer.enabled = false;
    }

    private IEnumerator ScanForPlayer()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    lastPlayerPosition = collider.transform.position;
                    playerDetected = true;

                    // Delay para que no sea la torreta la destructora de players 3000
                    yield return new WaitForSeconds(playerPositionDelay); 
                    yield return StartCoroutine(ShootLaser());

                    playerDetected = false;
                    break; 
                }
            }

            yield return null;
        }
    }

    private IEnumerator ShootLaser()
    {
        if (canShoot)
        {
            canShoot = false;
            laserLine.enabled = true;

            Vector3 directionToPlayer = lastPlayerPosition - boca.position; // Utilizar la última posición conocida
            directionToPlayer.Normalize();
            
            RaycastHit hit;

            if (Physics.Raycast(boca.position, directionToPlayer, out hit, laserMaxLength, playerLayer))
            {
                //Debug.Log("ahh disparie");
                laserLine.SetPosition(0, boca.position);
                laserLine.SetPosition(1, hit.point);
                // EFECTO DE SONIDO
                laserSound.Play();

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    //Debug.Log("ahh diablo me dio");
                    Player.instance.kill();
                }
            }
            else
            {
                laserLine.SetPosition(0, boca.position);
                laserLine.SetPosition(1, boca.position + directionToPlayer * laserMaxLength);
            }

            yield return null;

            laserLine.enabled = false;
            yield return new WaitForSeconds(shootingCooldown);
            canShoot = true;
        }
    }

    private void RotateTowardsPlayer()
    {
        if (Player.instance != null && !isStunned)
        {
            Vector3 directionToPlayer = Player.instance.transform.position - transform.position;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


    // Stunear a la torreta :o

    private IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(turretStunTime);

        // Reiniciar la rotación y el escaneo
        isStunned = false;
        recoverStunSound.Play();
        StartCoroutine(ScanForPlayer());
    }

    public void StunTurret()
    {
        if (isStunned)
        {
            Debug.Log("Stuneada LaserTurret");
            // Detener la rotación y el escaneo
            StopAllCoroutines();

            if (stunParticles != null)
            {
                stunParticles.Play();
            }

            if (stunSound != null)
            {
                stunSound.Play();
            }

            // Esperar el tiempo de aturdimiento
            StartCoroutine(RecoverFromStun());
            isStunned = false;
        }
    }
}

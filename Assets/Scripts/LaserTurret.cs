using System.Collections;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public Transform player;
    public GameObject boca; // El punto de origen del láser

    [Header("Variables")]
    public float detectionRadius = 10f;
    public float rotationSpeed = 5f;
    public float shootingCooldown = 4f;
    public float chargeDuration = 2f;
    public float laserSpeed = 2f;
    public Player playerScript;

    [Header("Sonidos")]
    public AudioClip greenLaserSound;
    public AudioClip redLaserSound;
    public AudioClip hitPlayerSound;
    private AudioSource audioSource;

    private bool playerDetected = false;
    private bool canShoot = true;
    private WaitForSeconds chargeDurationWait;
    private WaitForSeconds shootingCooldownWait;

    private LineRenderer laserLine;
    private Material greenLaserMaterial;
    private Material redLaserMaterial;

    private void Start()
    {
        greenLaserMaterial = new Material(Shader.Find("Standard"));
        greenLaserMaterial.color = Color.green;

        redLaserMaterial = new Material(Shader.Find("Standard"));
        redLaserMaterial.color = Color.red;

        chargeDurationWait = new WaitForSeconds(chargeDuration);
        shootingCooldownWait = new WaitForSeconds(shootingCooldown);

        if (Player.instance == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el LineRenderer
        laserLine = gameObject.AddComponent<LineRenderer>();
        ConfigureLineRenderer(laserLine, greenLaserMaterial);
    }

    private void ConfigureLineRenderer(LineRenderer lineRenderer, Material material)
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = material;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        DetectPlayer();
        RotateTowardsPlayer();

        if (playerDetected && canShoot)
        {
            StartCoroutine(ChargeAndShoot());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player = collider.transform;
                playerDetected = true;
                return;
            }
        }

        playerDetected = false;
    }

    private void RotateTowardsPlayer()
    {
        if (playerDetected)
        {
            transform.LookAt(player.position);
        }
    }

    private IEnumerator ChargeAndShoot()
    {
        canShoot = false;

        // Cargar láser verde
        laserLine.enabled = true;
        laserLine.material = greenLaserMaterial;
        audioSource.PlayOneShot(greenLaserSound);
        yield return chargeDurationWait;
        laserLine.enabled = false;

        // Disparar láser rojo
        laserLine.enabled = true;
        laserLine.material = redLaserMaterial;

        Vector3 laserDirection = player.position - boca.transform.position;
        laserDirection.Normalize();

        float elapsedTime = 0f;

        while (elapsedTime < shootingCooldown)
        {
            laserLine.SetPosition(0, boca.transform.position);
            laserLine.SetPosition(1, laserLine.GetPosition(1) + laserDirection * laserSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        laserLine.enabled = false;
        audioSource.PlayOneShot(redLaserSound);

        // Verificar si el láser rojo colisiona con el jugador
        RaycastHit hit;
        if (Physics.Raycast(boca.transform.position, laserDirection, out hit, shootingCooldown))
        {
            if (hit.collider.CompareTag("Player"))
            {
                audioSource.PlayOneShot(hitPlayerSound);
                playerScript.kill();
            }
        }

        yield return shootingCooldownWait;
        canShoot = true;
    }
}

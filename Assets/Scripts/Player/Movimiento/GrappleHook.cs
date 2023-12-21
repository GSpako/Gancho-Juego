using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    private SpringJoint springjoint;
    private LineRenderer rope;
    private Transform collision_transform;
    private Vector3 collision_transform_initPosition;
    private Vector3 collPos;
    private GameObject indicator;
    public Material mat;

    public bool grapling;


    [Header("References")]
    [SerializeField] private GameObject grappleGunTip;
    [SerializeField] private PauseMenuScript pauseMenuScript;
    [SerializeField] private GameObject ganchoPOV;


    [Header("Parameters")]
    [SerializeField] private float grappleLength;
    public float ropewidth = 0.01f;
    public Color ropecolor = Color.black;
    private GameObject hookSphere;
    public GameObject hookPrefab;
    private ParticleSystem particleHook;
    private Vector3 hitNormal;

    [Header("Donde puede engancharse el gancho")]
    public LayerMask enganchables;

    [Header("Inputs")]
    [SerializeField] private KeyCode grappleB1 = KeyCode.Mouse0;
    [SerializeField] private KeyCode grappleB2 = KeyCode.Mouse1;


    private void Start()
    {
        pauseMenuScript = CanvasReferences.instance.pause_script;
        indicator = CanvasReferences.instance.indicator;
        grapling = false;
    }
    private void Update()
    {
        if(!GameManager.Instance.pauseMenuScript.isGamePaused)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), grappleLength, enganchables)) {
                indicator.SetActive(true);
            }
            else
            {
                indicator.SetActive(false);
            }
            if (Input.GetKeyDown(grappleB1))
            {
                throwGrapple();
                PlayerAudioManager.instance.PlayHookSound();
            }
            if (Input.GetKeyUp(grappleB1))
            {
                stopGrapple();
            }
            if (rope != null)
            {
                
                rope.SetPosition(0, grappleGunTip.transform.position);
            }
        }

        if (collision_transform != null && rope != null && springjoint != null) {
            
            Vector3 newPos = collPos + collision_transform.position - collision_transform_initPosition;
            rope.SetPosition(1, newPos);
            springjoint.connectedAnchor = newPos;

            //Ahora debajo...
            // Hola soy Antonio lo volvi a poner arriba 
            Visualize(newPos, -hitNormal);
        }
    }

    public void throwGrapple() {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, grappleLength, enganchables))
        {
            grapling = true;
            ganchoPOV.SetActive(false);

            Vector3 pos = hit.point;

            collPos = pos;
            //InitPos
            collision_transform_initPosition = hit.collider.transform.position;
            collision_transform = (hit.collider.transform);

            Debug.DrawLine(start: Camera.main.transform.position, end: pos, duration: 0.1f, color: Color.white, depthTest: true);

            springjoint = gameObject.AddComponent<SpringJoint>();
            springjoint.autoConfigureConnectedAnchor = false;

            springjoint.connectedAnchor = pos;

            

            float distance = Vector3.Distance(transform.position, pos);
            springjoint.minDistance = distance * 0.01f;
            springjoint.maxDistance = distance * 0.8f;

            springjoint.spring = 4.5f;
            springjoint.damper = 7f;
            springjoint.massScale = 4.5f;

            rope = gameObject.AddComponent<LineRenderer>();
            rope.startWidth = ropewidth;
            rope.endWidth = ropewidth;
            rope.SetPositions(new Vector3[] { grappleGunTip.transform.position, pos});
            rope.material.SetColor("_Color",ropecolor);
            rope.material = mat;
            rope.endColor = ropecolor;


            if (hit.collider.CompareTag("Turret"))
            {
                LaserTurret.isStunned = true;
                //Debug.Log("Stuneada grapple");
            }


            //Visualize(pos, -hit.normal);
            hitNormal = hit.normal;

        }
        else Debug.Log("Demasiado lejos!");
    }
    public void stopGrapple()
    {
        collision_transform = null;
        ganchoPOV.SetActive(true);

        grapling = false;
        Destroy(springjoint);
        Destroy(rope);
        Destroy(hookSphere);
    }
    void Visualize(Vector3 pos, Vector3 normal) {
        // Para que no hayan infinitas
        if (hookSphere == null)
        {
            hookSphere = Instantiate(hookPrefab);
            hookSphere.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            hookSphere.GetComponent<SphereCollider>().enabled = false; // sino si dispara encima tuya tocas xd
            if(particleHook != null)
            {
                particleHook.Play();
            }
            //hookSphere.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        hookSphere.transform.position = pos;

        // Calcula la rotación para que el gancho mire hacia la superficie
        //Vector3 direction = pos - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(direction);

        //Lo he cambiado para que mire a la normal de la superficie, así no mira solo al personaje
        Quaternion rotation = Quaternion.LookRotation(normal);

        // Solo rota en el eje X
        hookSphere.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x + -90, rotation.eulerAngles.y, rotation.eulerAngles.z);
    }
}

using System.Collections;
using UnityEditor;
using UnityEngine;

public class SpringTrap : MonoBehaviour
{

    [Header("Variables")]
    public float fuerzaDeSalto = 10f; 
    public float cooldown = 3f;
    private Player player;

    [Header("Muelle malo >_<")]
    public bool esMuelleBueno = true;
    public float tiempoDeVidaMuelleMalo = 1f;
    private Animator animatorPadre; // El animador de MuelleCuadradado el papito
    public Vector3 direccionDeLanzamiento = Vector3.up; // Dirección de lanzamiento modificable desde el editor
    private float gizmoLineaLongitud = 10f; // Longitud de la línea del gizmo


    // AVISO AVISO
    // gracias por leer :D


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animatorPadre = GetComponentInParent<Animator>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animatorPadre.SetFloat("SaltoMuelles", 1.1f);
            StartCoroutine(ResetearSaltoMuelles());
            if (esMuelleBueno)
            {
                LanzarJugador();
                StartCoroutine(ActivarCooldown());
            } else if (!esMuelleBueno) 
            {
                LanzarJugadorMuelleMalo(direccionDeLanzamiento);
                StartCoroutine(ActivarCooldown());
            }
        }
    }

    private void LanzarJugador()
    {
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
    }

    // SE TOCA EN EL ESFERA 001 ES DONDE ESTA EL SCRIPT AHHHH, LO SIENTO NO SABIA DE OTRA MANERA EL COLLIDER
    private void LanzarJugadorMuelleMalo(Vector3 direccionDeLanzamiento)
    {
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(direccionDeLanzamiento * fuerzaDeSalto, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {   
        // Comento por error de build baia
        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(direccionDeLanzamiento), gizmoLineaLongitud, EventType.Repaint);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    // Ya no mata el muelle, tuvo su arco de redencion
    private void LanzarMatarJugador()
    {
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto * 50f, ForceMode.Impulse);
    }

    // Ya no mata el muelle, tuvo su arco de redencion
    private IEnumerator MatarJugadorDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoDeVidaMuelleMalo);
        Player.instance.kill();
    }
    private IEnumerator ResetearSaltoMuelles()
    {
        yield return new WaitForSeconds(1.0f); // Espera 1 segundo
        animatorPadre.SetFloat("SaltoMuelles", 0.0f);
    }
}

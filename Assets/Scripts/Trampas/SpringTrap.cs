using System.Collections;
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
                LanzarMatarJugador();
                StartCoroutine(MatarJugadorDespuesDeTiempo());
            }
        }
    }

    private void LanzarJugador()
    {
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    private void LanzarMatarJugador()
    {
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto * 50f, ForceMode.Impulse);
    }

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

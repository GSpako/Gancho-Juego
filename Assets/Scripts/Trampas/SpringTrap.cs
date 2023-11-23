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
    public Animator animator; // El animador de MuelleCuadradado el papito


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
        animator.SetBool("SaltoMuelle", true);
        PlayerAudioManager.instance.PlayMuelleSound();
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        animator.SetBool("SaltoMuelle", false);
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
}

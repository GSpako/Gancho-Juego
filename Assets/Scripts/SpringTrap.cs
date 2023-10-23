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


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    private void LanzarMatarJugador()
    {
        Player.instance.GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaDeSalto * 50f, ForceMode.Impulse);
    }

    private IEnumerator MatarJugadorDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoDeVidaMuelleMalo);
        Player.instance.kill();
    }
}

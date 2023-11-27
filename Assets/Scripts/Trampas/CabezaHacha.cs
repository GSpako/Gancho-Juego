using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CabezaHacha : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] private float tiempoParaMatarJugador = 1.5f;
    public float fuerzaDeLanzamiento = 100f;
    public AxeTrap axeTrap;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direccionImpacto = collision.contacts[0].normal;
            LanzarJugador(direccionImpacto);
            StartCoroutine(MatarJugadorDespuesDeTiempo());

        }
        else if (collision.gameObject.CompareTag("Trampa")) 
        {
            Debug.Log("Trampa collision");
            Vector3 direccionImpacto = collision.contacts[0].normal;
            LanzarTrampa(direccionImpacto, collision);
        }
    }

    private IEnumerator MatarJugadorDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoParaMatarJugador);
        Player.instance.kill();
    }

    private void LanzarJugador(Vector3 direccionImpacto)
    {
        Vector3 direccionLanzamiento = -direccionImpacto.normalized;
        Player.instance.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerzaDeLanzamiento, ForceMode.Impulse);
    }


    private void LanzarTrampa(Vector3 direccionImpacto, Collision collision)
    {
        Vector3 direccionLanzamiento = -direccionImpacto.normalized;
        Debug.Log("Trampa lanzada");
        collision.gameObject.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerzaDeLanzamiento, ForceMode.Impulse);
    }



}

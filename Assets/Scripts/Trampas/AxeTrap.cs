using System.Collections;
using UnityEngine;

public class AxeTrap : MonoBehaviour
{
    [Header("Variables")]
    public float fuerzaDeLanzamiento = 10f;
    public float cooldownOscilacion = 3f;
    public float tiempoParaMatarJugador = 2f; // Tiempo antes de que el hacha mate al jugador
    public float oscilacionMaxima = 90f; 
    public float velocidadOscilacion = 45f;
    private GameObject cabezaHacha, cuerpoHacha;

    private void Awake()
    {
        StartCoroutine(OscilarAxe());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LanzarJugador();
            StartCoroutine(ActivarCooldown());
            StartCoroutine(MatarJugadorDespuesDeTiempo());
        }
    }

    private void LanzarJugador()
    {
        Vector3 direccionAleatoria = Random.onUnitSphere; // Genera una dirección aleatoria en el espacio tridimensional
        Player.instance.GetComponent<Rigidbody>().AddForce(direccionAleatoria * fuerzaDeLanzamiento, ForceMode.Impulse);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldownOscilacion);
        StartCoroutine(OscilarAxe()); // Reiniciar la oscilación después del cooldown
    }

    private IEnumerator OscilarAxe()
    {
        while (true)
        {
            float anguloActual = Mathf.PingPong(Time.time * velocidadOscilacion, oscilacionMaxima * 2) - oscilacionMaxima;
            transform.localRotation = Quaternion.Euler(anguloActual, 0f, 0f);
            yield return null;
        }
    }

    private IEnumerator MatarJugadorDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoParaMatarJugador);
        Player.instance.kill();
    }
}

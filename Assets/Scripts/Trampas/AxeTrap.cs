using System.Collections;
using UnityEngine;

public class AxeTrap : MonoBehaviour
{
    [Header("Variables")]
    public float fuerzaDeLanzamiento = 10f;
    public float cooldownOscilacion = 3f;
    public float oscilacionMaxima = 90f; 
    public float velocidadOscilacion = 45f;
    public float anguloActual;

    private void Awake()
    {
        StartCoroutine(OscilarAxe());
    }


    private void LanzarJugador()
    {
        Vector3 direccionAleatoria = Random.onUnitSphere; // Genera una dirección aleatoria en el espacio tridimensional
        Player.instance.GetComponent<Rigidbody>().AddForce(direccionAleatoria * fuerzaDeLanzamiento, ForceMode.Impulse);
    }

    private IEnumerator OscilarAxe()
    {
        while (true)
        {
            anguloActual = Mathf.PingPong(Time.time * velocidadOscilacion, oscilacionMaxima * 2) - oscilacionMaxima;
            transform.localRotation = Quaternion.Euler(anguloActual, 0f, 0f);
            yield return null;
        }
    }

    public Vector3 GetDireccionLanzamiento()
    {
        // Devuelve la dirección deseada, por ejemplo, la derecha (Vector3.right)
        return transform.right;
    }
}

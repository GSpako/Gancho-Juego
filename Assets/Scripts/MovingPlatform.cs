using System.Collections;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    // Variables específicas para la plataforma que desaparece y aparece.
    public float tiempoDesaparece = 2f;
    public float tiempoReaparece = 1f;

    // Variable para determinar si la plataforma es mala y desaparece.
    public bool esPlataformaDesaparece = false;
    public bool desaparecePlataforma = false;

    private void Start()
    {
        // Inicializar la posición de la plataforma al puntoA.
        transform.position = puntoA.position;
    }

    private void Update()
    {
        // Mover la plataforma entre los puntos A y B.
        transform.position = Vector3.Lerp(puntoA.position, puntoB.position, Mathf.PingPong(Time.time * velocidad, 1));
    }

    private void OnDrawGizmos()
    {
        // Dibujar una línea entre puntoA y puntoB en el editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(puntoA.position, puntoB.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (desaparecePlataforma && esPlataformaDesaparece && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DesaparecerYReaparecer());
        }
    }

    private IEnumerator DesaparecerYReaparecer()
    {
        // Desaparecer la plataforma.
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(tiempoDesaparece);

        // Reaparecer la plataforma.
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(tiempoReaparece);
    }
}

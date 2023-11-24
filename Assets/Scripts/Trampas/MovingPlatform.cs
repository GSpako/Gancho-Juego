using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Referencias")]
    public Transform puntoA;
    public Transform puntoB;
    [Header("Variables")]
    public float velocidad = 2f;

    private Vector3 oldPos;
    private Vector3 newPos;

    private bool playerOnTop;

    private void Start()
    {
        // Inicializar la posición de la plataforma al puntoA.
        transform.position = puntoA.position;
    }

    private void FixedUpdate()
    {
        // Mover la plataforma entre los puntos A y B.
        transform.position = Vector3.Lerp(puntoA.position, puntoB.position, Mathf.PingPong(TimerSystem.instance.getTime() * velocidad, 1));
    }
    private void OnDrawGizmos()
    {
        // Dibujar una línea entre puntoA y puntoB en el editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(puntoA.position, puntoB.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            oldPos = transform.position;
            Player.instance.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnTop = false;
            Player.instance.transform.SetParent(null);
        }
    }
}

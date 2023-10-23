using System.Collections;
using UnityEngine;

public class SpringTrap : MonoBehaviour
{
    public float fuerzaDeSalto = 10f; // Fuerza con la que el jugador será lanzado.
    public float cooldown = 3f; // Tiempo de espera antes de que el muelle pueda ser activado nuevamente.
    public bool esMuelleBueno = true; // Indica si es un muelle bueno o malo.
    public float tiempoDeVidaMuelleMalo = 1f; // Tiempo que tarda en matar al jugador (solo para muelles malos).

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (esMuelleBueno)
            {
                // Lanza al jugador hacia arriba.
                LanzarJugador(collision.gameObject.GetComponent<Rigidbody2D>());
                // Inicia el cooldown.
                StartCoroutine(ActivarCooldown());
            }
            else
            {
                // Lanza al jugador fuertemente hacia arriba y lo mata después de un tiempo.
                LanzarMatarJugador(collision.gameObject.GetComponent<Player>());
            }
        }
    }

    private void LanzarJugador(Rigidbody2D jugadorRb)
    {
        jugadorRb.velocity = new Vector2(jugadorRb.velocity.x, 0f); // Resetea la velocidad vertical del jugador.
        jugadorRb.AddForce(Vector2.up * fuerzaDeSalto, ForceMode2D.Impulse);
    }

    private IEnumerator ActivarCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        // El muelle está listo para ser activado nuevamente.
    }

    private void LanzarMatarJugador(Player jugador)
    {
        // Lanza al jugador fuertemente hacia arriba.
        jugador.GetComponent<Rigidbody2D>().AddForce(Vector2.up * fuerzaDeSalto * 5f, ForceMode2D.Impulse);

        // Mata al jugador después de un tiempo.
        StartCoroutine(MatarJugadorDespuesDeTiempo(jugador));
    }

    private IEnumerator MatarJugadorDespuesDeTiempo(Player jugador)
    {
        yield return new WaitForSeconds(tiempoDeVidaMuelleMalo);
        jugador.kill();
    }
}

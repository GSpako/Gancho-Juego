using UnityEngine;

public class FlechaIndicadora : MonoBehaviour
{
    [Header("Configuración")]
    public Material materialFlecha;
    public Color colorFlecha = Color.green;
    public float tamañoFlecha = 1.0f;

    private void OnDrawGizmos()
    {
        DrawArrow();
    }

    private void DrawArrow()
    {
        Gizmos.color = colorFlecha;
        Gizmos.DrawRay(transform.position, transform.forward * tamañoFlecha);

        float size = tamañoFlecha * 0.2f;
        float coneSize = tamañoFlecha * 0.6f;

        // Dibujar el cuerpo de la flecha
        Gizmos.DrawWireCube(transform.position + transform.forward * tamañoFlecha, new Vector3(size, size, tamañoFlecha - coneSize));

        // Dibujar la cabeza de la flecha (cono)
        Gizmos.DrawCone(transform.position + transform.forward * tamañoFlecha, transform.forward * coneSize, 0);
    }
}

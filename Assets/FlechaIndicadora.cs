using UnityEngine;

public class FlechaIndicadora : MonoBehaviour
{
    [Header("Configuraci�n")]
    public Material materialFlecha;
    public Color colorFlecha = Color.green;
    public float tama�oFlecha = 1.0f;

    private void OnDrawGizmos()
    {
        DrawArrow();
    }

    private void DrawArrow()
    {
        Gizmos.color = colorFlecha;
        Gizmos.DrawRay(transform.position, transform.forward * tama�oFlecha);

        float size = tama�oFlecha * 0.2f;
        float coneSize = tama�oFlecha * 0.6f;

        // Dibujar el cuerpo de la flecha
        Gizmos.DrawWireCube(transform.position + transform.forward * tama�oFlecha, new Vector3(size, size, tama�oFlecha - coneSize));

        // Dibujar la cabeza de la flecha (cono)
        Gizmos.DrawCone(transform.position + transform.forward * tama�oFlecha, transform.forward * coneSize, 0);
    }
}

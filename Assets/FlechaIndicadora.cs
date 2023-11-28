using UnityEngine;

public class FlechaIndicadora : MonoBehaviour
{
    [Header("Configuraci�n Flecha")]
    public Color colorFlecha = Color.green;
    public float tama�oFlecha = 1.0f;
    public float grosorFlecha = 0.1f;  // Grosor de la flecha

    private void OnDrawGizmos()
    {
        DrawArrow();
    }

    private void Update()
    {
        DrawArrow();
    }

    private void DrawArrow()
    {
        Gizmos.color = colorFlecha;
        Gizmos.DrawRay(transform.position, transform.forward * tama�oFlecha);
        DrawArrowHead(transform.position + transform.forward * tama�oFlecha, transform.forward, grosorFlecha);
    }

    private void DrawArrowHead(Vector3 position, Vector3 direction, float thickness)
    {
        float arrowHeadLength = 0.25f * tama�oFlecha;
        float arrowHeadAngle = 20.0f;

        // Calcula la posici�n de la punta de la flecha
        Vector3 arrowHeadPos = position - direction * arrowHeadLength;

        // Calcula la rotaci�n de la punta de la flecha
        Quaternion arrowHeadRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0);

        // Dibuja la punta de la flecha
        Gizmos.DrawRay(position, arrowHeadRotation * Vector3.up * thickness);
        Gizmos.DrawRay(position, arrowHeadRotation * Vector3.down * thickness);
    }
}

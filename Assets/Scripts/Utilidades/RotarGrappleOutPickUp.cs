using UnityEngine;

public class RotarGrappleOutPickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;  // Velocidad de rotaci�n en grados por segundo
    [SerializeField] private float minHeight = 1f;       // Altura m�nima de flotaci�n
    [SerializeField] private float maxHeight = 3f;       // Altura m�xima de flotaci�n  
    [SerializeField] private float floatingSpeed = 1f;   // Velocidad de flotaci�n

    private void Update()
    {
        // Rotar el objeto alrededor de su eje Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Mover el objeto hacia arriba y abajo
        float newY = Mathf.PingPong(Time.time * floatingSpeed, maxHeight - minHeight) + minHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

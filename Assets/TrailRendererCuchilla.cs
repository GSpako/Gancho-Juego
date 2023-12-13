using UnityEngine;

public class TrailRendererManager : MonoBehaviour
{
    private float trailTime = 1f;
    private float startWidth = 1.0f;
    private float endWidth = 0f;
    private Material trailMaterial;
    private Color startColor = Color.white;
    private Color endColor = Color.clear;

    private TrailRenderer trailRenderer;
    private GameObject trailingObject;

    void Start()
    {
        // Crear un objeto secundario que seguirá la rotación del objeto principal
        trailingObject = new GameObject("TrailingObject");
        trailingObject.transform.parent = transform;
        trailingObject.transform.localPosition = Vector3.zero;
        //pene

        // Agregar TrailRenderer al objeto secundario
        trailRenderer = trailingObject.AddComponent<TrailRenderer>();

        // Configurar propiedades del TrailRenderer
        trailRenderer.time = trailTime;
        trailRenderer.startWidth = startWidth;
        trailRenderer.endWidth = endWidth;
        trailRenderer.material = trailMaterial;
        trailRenderer.startColor = startColor;
        trailRenderer.endColor = endColor;

        // Otras configuraciones posibles (ajusta según tus necesidades):
        trailRenderer.numCornerVertices = 0;
        trailRenderer.numCapVertices = 0;
        trailRenderer.alignment = LineAlignment.View;
        trailRenderer.autodestruct = false;
        trailRenderer.emitting = true;
    }

    void Update()
    {
        // Seguir la posición y rotación del objeto principal
        trailingObject.transform.position = transform.position;
        trailingObject.transform.rotation = transform.rotation;
    }
}

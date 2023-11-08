using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMessage : MonoBehaviour
{
    [SerializeField] string mensaje;

    private void OnTriggerEnter(Collider other)
    {
        print("alguien entraaa");
        if (other.CompareTag("Player")) {
            print("es un player...");
            CanvasBehaviour.instance.Log(mensaje,Color.cyan);
        }
    }
}

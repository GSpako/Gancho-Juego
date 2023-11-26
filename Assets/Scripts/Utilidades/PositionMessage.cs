using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMessage : MonoBehaviour
{
    [SerializeField] string mensaje;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            CanvasBehaviour.instance.Log(mensaje,Color.black);
        }
    }
}

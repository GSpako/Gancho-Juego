using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilador : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] Transform aspasRotator;


    // Start is called before the first frame update
    void Start()
    {
        Transform ventilador = aspasRotator = transform.Find("Ventilador");
        aspasRotator = ventilador.Find("Rotator").transform;

    }

    // Update is called once per frame
    void Update()
    {
        aspasRotator.Rotate(0,Time.deltaTime * 50 * force,0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
            Rigidbody rb = Player.instance.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * force);
        }
    }
}

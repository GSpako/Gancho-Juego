using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilador : MonoBehaviour
{
    [SerializeField] float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
            Rigidbody rb = Player.instance.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * force);
        }
    }
}

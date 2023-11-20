using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] float startDelay = 1;
    [SerializeField] float angle = 45;
    [SerializeField] float speed = 1;

    [SerializeField] bool inverse = false;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startDelay && !inverse) {
            transform.localRotation = Quaternion.Euler(0f, 0f, angle * Mathf.Sin((Time.time - startDelay) * speed));
        } 
        else if (Time.time > startDelay && inverse) {
            transform.localRotation = Quaternion.Euler(0f, 0f, angle * Mathf.Cos((Time.time - startDelay) * speed));
        }
    }
}

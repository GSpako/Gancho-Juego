using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] float startDelay = 1;
    [SerializeField] float angle = 45;
    [SerializeField] float speed = 1;

    [SerializeField] bool pos = false;
    [SerializeField] bool rot = false;

    [SerializeField] bool inverse = false;

    [SerializeField] bool ejeX = false;
    [SerializeField] bool ejeY = false;
    [SerializeField] bool ejeZ = false;


    // Update is called once per frame
    void Update()
    {
        float movimiento, x, y, z;

        if (inverse) {
            movimiento = angle * Mathf.Sin((Time.time - startDelay) * speed);
        } else {
            movimiento = angle * Mathf.Cos((Time.time - startDelay) * speed);
        }

        if (ejeX) { x = movimiento; } else { x = 0f; }
        if (ejeY) { y = movimiento; } else { y = 0f; }
        if (ejeZ) { z = movimiento; } else { z = 0f; }

        if (Time.time > startDelay)
            if (pos) {
                transform.localPosition = new Vector3(x, y, z);
            }
            if (rot)
            {
                transform.localRotation = Quaternion.Euler(x, y, z);
            }


    }
}

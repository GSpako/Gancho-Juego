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
        float movimiento, xP, yP, zP, xR, yR, zR;

        if (inverse) {
            movimiento = angle * Mathf.Sin((Time.time - startDelay) * speed);
        } else {
            movimiento = angle * Mathf.Cos((Time.time - startDelay) * speed);
        }

        if (ejeX && pos) { xP = movimiento + transform.localPosition.x; } else { xP = transform.localPosition.x; }
        if (ejeY && pos) { yP = movimiento + transform.localPosition.y; } else { yP = transform.localPosition.y; }
        if (ejeZ && pos) { zP = movimiento + transform.localPosition.z; } else { zP = transform.localPosition.z; }

        if (ejeX && rot) { xR = movimiento + transform.localRotation.x; } else { xR = transform.localRotation.x; }
        if (ejeY && rot) { yR = movimiento + transform.localRotation.y; } else { yR = transform.localRotation.y; }
        if (ejeZ && rot) { zR = movimiento + transform.localRotation.z; } else { zR = transform.localRotation.z; }


        if (Time.time > startDelay)
            if (pos) {
                transform.localPosition = new Vector3(xP, yP, zP);
            }
            if (rot)
            {
                transform.localRotation = Quaternion.Euler(xR, yR, zR);
            }


    }
}

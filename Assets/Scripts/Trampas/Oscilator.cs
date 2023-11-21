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

    float startXP, startYP, startZP;
    float startXR, startYR, startZR;

    void Start() {
        startXP = transform.localPosition.x;
        startYP = transform.localPosition.y;
        startZP = transform.localPosition.z;

        startXR = transform.localRotation.x;
        startYR = transform.localRotation.y;
        startZR = transform.localRotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float movimiento, xP, yP, zP, xR, yR, zR;

        if (inverse) {
            movimiento = angle * Mathf.Sin((Time.time - startDelay) * speed);
        } else {
            movimiento = angle * Mathf.Cos((Time.time - startDelay) * speed);
        }

        if (ejeX && pos) { xP = movimiento + startXP; } else { xP = transform.localPosition.x; }
        if (ejeY && pos) { yP = movimiento + startYP; } else { yP = transform.localPosition.y; }
        if (ejeZ && pos) { zP = movimiento + startZP; } else { zP = transform.localPosition.z; }

        if (ejeX && rot) { xR = movimiento + startXR; } else { xR = transform.localRotation.x; }
        if (ejeY && rot) { yR = movimiento + startYR; } else { yR = transform.localRotation.y; }
        if (ejeZ && rot) { zR = movimiento + startZR; } else { zR = transform.localRotation.z; }

        if (pos) {
            transform.localPosition = new Vector3(xP, yP, zP);
        }
        if (rot)
        {
            transform.localRotation = Quaternion.Euler(xR, yR, zR);
        }


    }
}

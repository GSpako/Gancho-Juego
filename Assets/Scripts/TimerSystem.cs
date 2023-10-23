using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    [SerializeField] float totalTime;
    [SerializeField] BulletTime bT;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > (totalTime + startTime))
        {
            bT.bloquearMenus = true;
            //activar la pantalla de muerte
        }
    }
}

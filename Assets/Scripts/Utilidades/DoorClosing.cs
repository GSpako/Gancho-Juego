using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorClosing : MonoBehaviour
{
    const double GoldenRatio = 1.61803398874989484820458683436;

    [SerializeField] GameObject door;
    [Space]
    [SerializeField] Vector3 initSize;
    [SerializeField] Vector3 endSize;
    [Space]
    [SerializeField] Vector3 initPos;
    [SerializeField] Vector3 endPos;
    [Space]
    [Header("Retraso inicio cerrado")]
    [SerializeField] float parametroEspera = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerSystem.instance.timerRunning && !TimerSystem.instance.not_Active) {
            float actualTime = TimerSystem.instance.getTime();
            float max = TimerSystem.instance.getMaxTime();
            float percentage = actualTime / max;
            float specialLerp = Mathf.Exp(-parametroEspera*(1-percentage));
            door.transform.localPosition = Vector3.Lerp(initPos, endPos, specialLerp);
            door.transform.localScale = Vector3.Lerp(initSize, endSize, specialLerp);
        }
    }
}

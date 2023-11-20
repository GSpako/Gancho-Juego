using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarColorSala : MonoBehaviour
{
    [Header("Singleton")]
    public static CambiarColorSala instance;

    [Header("Referencias")]
    public Material materialACambiarElColor;
    public Color colorStart;
    public Color colorEnd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, TimerSystem.instance.getMaxTime()) / TimerSystem.instance.getMaxTime();
        materialACambiarElColor.color = Color.Lerp(colorStart, colorEnd, lerp);
    }
}

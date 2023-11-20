using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CambiarColorSala : MonoBehaviour
{
    [Header("Singleton")]
    public static CambiarColorSala instance;

    [Header("Referencias")]
    public Material materialACambiarElColor;
    [Header("Variables uwu")]
    private Color colorInicial; // Para almacenar el color inicial
    public Color colorStart;    // Color inicial
    public Color colorEnd;      // Color final del material

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

        colorInicial = materialACambiarElColor.color;
    }

    private void Update()
    {
        float actualTime = TimerSystem.instance.getRemainingTime();
        float max = TimerSystem.instance.getMaxTime();
        float percentage = actualTime / max;
        materialACambiarElColor.color = Color.Lerp(colorStart, colorEnd, 1 - percentage);
    }

    // Función para restaurar el color inicial del material :O
    public void RestaurarColorInicial()
    {
        materialACambiarElColor.color = colorInicial;
    }
}

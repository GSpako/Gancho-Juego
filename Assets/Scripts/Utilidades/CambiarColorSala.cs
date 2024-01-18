using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarColorSala : MonoBehaviour
{
    [Header("Singleton")]
    public static CambiarColorSala instance;

    [Header("Variables uwu")]
    public Color colorStart;    // Color inicial
    public Color colorEnd;      // Color final del material
    [Tooltip("Poner el nombre del material que quieres cambiar, y aplicar ese material a varias cosas")]
    public string nombreMaterialACambiar = "ParedesSala12";

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

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

        // Buscar todas las paredes con el material "ParedesSala"
        EncontrarParedesSala();
    }

    private void Update()
    {
        float actualTime = TimerSystem.instance.getRemainingTime();
        float max = TimerSystem.instance.getMaxTime();
        float percentage = actualTime / max;

        // Modificar el color de todas las paredes encontradas
        foreach (var renderer in originalColors.Keys)
        {
            renderer.material.color = Color.Lerp(colorStart, colorEnd, 1 - percentage);
        }
    }

    private void EncontrarParedesSala()
    {
        // Encontrar todos los Renderers con el material "nombreMaterialACambiar"
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            if (renderer.material.name.Contains(nombreMaterialACambiar))
            {
                // Almacenar el color original
                originalColors[renderer] = renderer.material.color;
            }
        }
    }

    // Función para restaurar el color original de todas las paredes :O
    public void RestaurarColoresOriginales()
    {
        foreach (var pair in originalColors)
        {
            pair.Key.material.color = pair.Value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasReferences : MonoBehaviour
{
    [Header("Singleton")]
    public static CanvasReferences instance;

    [Header("Referencias")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI movStateText;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}

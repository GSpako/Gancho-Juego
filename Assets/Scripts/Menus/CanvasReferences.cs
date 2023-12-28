using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasReferences : MonoBehaviour
{
    [Header("Singleton")]
    public static CanvasReferences instance;

    [Header("Referencias")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI movStateText;
    public Slider sloMo;
    public PauseMenuScript pause_script;
    public GameObject indicator;
    public Slider dash;
    public GameObject panelMuerte;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}

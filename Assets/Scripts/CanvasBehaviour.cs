using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour instance;
    [SerializeField] float messageTime = 1;
    [SerializeField] Color defColorcolor = Color.white;
    [SerializeField] TMPro.TMP_FontAsset fontStyle;
    [SerializeField] RectTransform mensajesRT;
    [SerializeField] Sprite textImage;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.LevelManager.canvas = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Log(string msg)
    {
        Log(msg, defColorcolor);
    }
    public void Log(string msg, Color c) { 
        GameObject img = new GameObject("MensajeImage", typeof(RectTransform));
        img.AddComponent<Image>();
        img.GetComponent<RectTransform>().SetParent(mensajesRT);
        GameObject go = new GameObject("MensajeText", typeof(RectTransform));
        RectTransform rt = go.GetComponent<RectTransform>();
        img.GetComponent<Image>().DOFade(0, messageTime * 1.6f);

        img.GetComponent<Image>().raycastTarget = false;
        img.GetComponent<Image>().sprite = textImage;
        img.GetComponent<RectTransform>().localScale = new Vector3(2, 1, 0);

        rt.GetComponent<RectTransform>().SetParent(img.GetComponent<RectTransform>());

        go.AddComponent<TextMeshProUGUI>().text = msg;
        go.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        go.GetComponent<TextMeshProUGUI>().DOFade(0, messageTime * 1.6f);
        go.GetComponent<TextMeshProUGUI>().font = fontStyle;
        go.GetComponent<TextMeshProUGUI>().fontSize = 30; // ANTONIO: En la build es chiquito antes era 16
        go.GetComponent<TextMeshProUGUI>().color = c;
        go.GetComponent<TextMeshProUGUI>().raycastTarget = false;

        rt.DOShakeAnchorPos(messageTime * 0.05f, strength:2, vibrato:10, randomness:6,snapping:false);
        //rt.DOLocalMove(new Vector2(-200, 0), messageTime*0.9f, false);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320f) ;
        rt.anchoredPosition = new Vector2(20, 5f); // para subir arriba la derecha un pelin el texto
        Destroy(img,messageTime);
    }
}

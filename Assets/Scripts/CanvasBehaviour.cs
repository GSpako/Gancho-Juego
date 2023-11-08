using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour instance;
    [SerializeField] float messageTime = 1;
    [SerializeField] Color defColorcolor = Color.white;
    [SerializeField] TMPro.TMP_FontAsset fontStyle;

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
        GameObject go = new GameObject("Mensaje",typeof(RectTransform));
        go.AddComponent<TextMeshProUGUI>().text = msg;
        go.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        go.GetComponent<TextMeshProUGUI>().DOFade(0, messageTime*0.9f);
        go.GetComponent<TextMeshProUGUI>().font = fontStyle;
        go.GetComponent<TextMeshProUGUI>().color = c;
        go.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        go.GetComponent<RectTransform>().DOLocalMove(new Vector2(0, 200), messageTime*0.9f, false);
        go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600f) ;
        go.GetComponent<TextMeshProUGUI>().raycastTarget = false;   
        Destroy(go,messageTime);
    }
}

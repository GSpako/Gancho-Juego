using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectorDeNivel : MonoBehaviour
{

    public GameObject buttonPrefab;
    public Transform levelsParetCo;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (string str in GameManager.Instance.levels) { 
            //Debug.Log(str);
            GameObject bot = Instantiate(buttonPrefab, levelsParetCo);
            bot.name = "Boton_" + str;
            bot.GetComponentInChildren<TextMeshProUGUI>().text = str;
            bot.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            int index = i;
            bot.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(index));
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonClicked(int num) {
        GameManager.Instance.SetLevel(num); 
    }
}

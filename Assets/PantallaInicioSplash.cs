using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaInicioSplash : MonoBehaviour
{
    [SerializeField] float time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CambioEscena", time);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey){
            CambioEscena();
        }
    }

    void CambioEscena()
    {
        SceneManager.LoadScene("Menu");
    }
}

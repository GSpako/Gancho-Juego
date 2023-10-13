using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    [SerializeField] public Slider slowmoCharge;
    [SerializeField] public GameObject sliderObject;
    [SerializeField] private float timeReductionRate;
    [SerializeField] private float decreaserate;
    [SerializeField] private float refilrate;
    private float rate;
    private bool slowMode = false;
    private bool bloquear = false;

    void Start()
    {
        slowmoCharge.value = slowmoCharge.maxValue;
    }

    void FixedUpdate()
    { 
        // Si la barra esta al maximo, quitarla, en caso contrario ponerla
        if(slowmoCharge.value >= slowmoCharge.maxValue*0.99f) {
            sliderObject.SetActive(false);
        } else {
            sliderObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //incrementar o decrementar carga
        slowmoCharge.value = slowmoCharge.value - rate*Time.deltaTime;

        //desbloquear si se ha cargado al maximo
        if(slowmoCharge.value == slowmoCharge.maxValue)
            bloquear = false;

        //bloquear si se ha gastado por completo
        if(slowmoCharge.value == slowmoCharge.minValue) {
            bloquear = true;
            desactivarSlow();
        }

        if(bloquear) {return;}
        
        if(Input.GetKeyDown(KeyCode.E) && slowmoCharge.value > slowmoCharge.minValue)
            slowMode = !slowMode;
        
        if(slowMode) {
            activarSlow();
        } else {
            desactivarSlow();
        }
    }

    void activarSlow() {
        slowMode = true;
        rate = decreaserate;
        Time.timeScale = timeReductionRate;
    }

    void desactivarSlow() {
        slowMode = false;
        rate = -refilrate;
        Time.timeScale = 1.0f;
    }
}

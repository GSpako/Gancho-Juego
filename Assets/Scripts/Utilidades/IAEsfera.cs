using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEsfera : MonoBehaviour
{
    const float DeltaTOAI = 0.1f;
    float TOAI = 0.0f;


    enum EsferaEstados { Idle, Wander, Attack, Flee}
    EsferaEstados esferaEstados;


    void Start()
    {
        esferaEstados = EsferaEstados.Idle;
    }


    void Update()
    {
        
    }
}

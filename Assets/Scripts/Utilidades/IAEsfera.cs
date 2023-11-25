using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEsfera : MonoBehaviour
{
    const float DeltaTOAI = 0.1f;
    float TOAI = 0.0f;
    //float vidaPersonaje = 100.0f;
    const float vidaPersonajeMaxima = 3.0f;

    bool inState;

    enum EsferaEstados { Idle, Wander, Attack, Flee}
    EsferaEstados esferaEstados;


    void Start()
    {
        ChangeState(EsferaEstados.Idle);
    }

    void ChangeState(EsferaEstados siguienteEstado)
    {
        esferaEstados = siguienteEstado;
        inState = true;
    }

    void Wander()
    {

    }

    bool SeeEnemy()
    {
        return true;
    }

    void Update()
    {
        if (TOAI > 0.0f)
        {
            TOAI -= Time.deltaTime;
        } else
        {
            TOAI = DeltaTOAI;
            switch (esferaEstados)
            {
                case EsferaEstados.Idle:
                    if(inState == true)
                    {
                        inState = false;
                        //vidaPersonaje = vidaPersonajeMaxima;
                    }

                    Wander();

                    if(SeeEnemy())
                    {
                        ChangeState(EsferaEstados.Wander);
                    }

                    break;
                case EsferaEstados .Wander:

                    break;
                case EsferaEstados.Attack: 

                    break;
                case EsferaEstados.Flee:

                    break;
            }
        }
    }
}

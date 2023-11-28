using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerAudioManager.instance.PlayMuelleSound();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        if (cameraPosition != null && Player.instance != null) {
            transform.position = Player.instance.GetComponent<PlayerMovement>().cameraPosition.position;

        }
    }




}

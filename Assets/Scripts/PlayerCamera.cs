using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [Header("Singleton")]
    public static PlayerCamera instance;

    [Header("Parameters")]
    public float sensibilityX;
    public float sensibilityY;

    public Transform orientation;
    public Transform camHolder;

    public float transitionTime = 0.25f;


    float xRotation, yRotation;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // en el medio y invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibilityY;

        yRotation += mouseX;
        xRotation -= mouseY;

        // no mas de 90�
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, transitionTime);
    }

    public void doTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, zTilt), transitionTime);
    }

    public void DoFovSlide(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, transitionTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Singleton")]
    public static Player instance;

    [Header("References")]
    public static PlayerCamera camera;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (camera == null)
        {
            if (PlayerCamera.instance != null)
                camera = PlayerCamera.instance;
            else
                camera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

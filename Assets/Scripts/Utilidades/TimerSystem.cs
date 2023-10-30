using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    public static TimerSystem instance;

    [SerializeField] float totalTime;
    [SerializeField] BulletTime bT;
    float startTime;
    public bool timerRunning;

    [SerializeField] float time;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bT == null)
            bT = GameObject.FindObjectOfType<BulletTime>();

        StartTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > (totalTime + startTime) && timerRunning)
        {
            bT.bloquearMenus = true;

            //activar la pantalla de muerte
            Player.instance.kill();
            timerRunning = false;   
        }
        if (timerRunning) {
            time = Time.time - startTime;
        }
    }

    public void StartTimer() { 
        timerRunning = true;
        startTime = Time.time;
    }

    public void ExitLevel() {
        CanvasBehaviour.instance.Log("Has escapado! (en " + time + " segundos)");
        Debug.Log("Has escapado!");
        timerRunning = false;
    }
}

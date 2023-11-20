using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameManager.GameState gs = GameManager.GameState.level;
    public Spawner spawner;
    public GameObject canvas;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.gameState = gs;
        GameManager.Instance.LevelManager = this;

        if (spawner == null) Debug.LogError("Falta el spawner");
        if (canvas == null) Debug.LogError("Falta el canvas");
        //GameManager.Instance.set
        if (Player.instance == null) { spawner.Spawn(Spawner.types.player); }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    
}

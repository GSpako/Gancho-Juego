using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;



public class VideoFinish : MonoBehaviour
{

    [SerializeField]
    VideoPlayer myVideoPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer.loopPointReached += GoToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void GoToMenu(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }

}

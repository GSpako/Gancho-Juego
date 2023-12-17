using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    [SerializeField] private Texture2D cursor_normal;
    [SerializeField] private Texture2D cursor_hover;

    [SerializeField] private Vector2 normal_hotspot;
    [SerializeField] private Vector2 hover_hotspot;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            Cursor.SetCursor(cursor_normal, normal_hotspot ,CursorMode.Auto);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cursor.SetCursor(cursor_hover, hover_hotspot, CursorMode.Auto);
        }

    }


    public void OnButtonEnter()
    {
        Cursor.SetCursor(cursor_hover, hover_hotspot, CursorMode.Auto);
    }

    public void OnButtonExit()
    {
        Cursor.SetCursor(cursor_normal, normal_hotspot, CursorMode.Auto);
    }

}

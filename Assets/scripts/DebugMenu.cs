using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public static float mouseSensetivity = 1.0f;

    private void OnGUI ()
    {
        GUI.Box(new Rect(10,10,100,90), "Debug Menu");

        mouseSensetivity = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), mouseSensetivity, 0.0f, 4.0f);
    
        if(GUI.Button(new Rect(20,40,80,20), "Esc to exit"))
        {
            Application.Quit(0);
        }
    }
}

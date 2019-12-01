using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_GUI2 : MonoBehaviour
{
    
    public int BoxFontSize;
    public int LabelFontSize;
    public int ButtonFontSize;
    public string TaskTitle;
    public string ControlTitle;
    public Texture2D temp;
    public List<Texture2D> crossList;

    void Start()
    {
    }
    
    void OnGUI()
    {
        GUI.skin.label.fontSize = LabelFontSize;
        GUI.skin.box.fontSize = BoxFontSize;
        GUI.skin.button.fontSize = ButtonFontSize;

        GUILayout.BeginArea (new Rect (Screen.width - 300, 500, 800, 390));
        GUI.Box(new Rect(0,0,300,390), TaskTitle);

        GUI.Label(new Rect(10,50, 30, 30), crossList[0]);
        GUI.Label(new Rect(50,55, 200, 30), "Move");

        GUI.Label(new Rect(10,90, 30, 30), crossList[1]);
        GUI.Label(new Rect(50,95, 200, 30), "Jump");

        GUI.Label(new Rect(10,130, 30, 30), crossList[2]);
        GUI.Label(new Rect(50,135, 200, 30), "Crouch");

        GUI.Label(new Rect(10,170, 30, 30), crossList[3]);
        GUI.Label(new Rect(50,175, 200, 30), "Pick Up / Swap");

        GUI.Label(new Rect(10,210, 30, 30), crossList[4]);
        GUI.Label(new Rect(50,215, 200, 30), "Shooting / Aim");

        GUI.Label(new Rect(10,250, 30, 30), crossList[5]);
        GUI.Label(new Rect(50,255, 200, 30), "Reload");

        GUI.Label(new Rect(10,290, 30, 30), crossList[6]);
        GUI.Label(new Rect(50,295, 200, 30), "Grenade");

        GUI.Label(new Rect(10,330, 30, 30), crossList[7]);
        GUI.Label(new Rect(50,335, 200, 30), "Find Partner");
        GUILayout.EndArea();

        //Gui
        GUILayout.BeginArea (new Rect (0,500, 700, 200));
        GUI.Box(new Rect(0,0,150,220), ControlTitle);

        GUI.Label(new Rect(10,25, 150, 30), "Move - WASD");
        GUI.Label(new Rect(10,50, 150, 30), "Jump - Space");
        GUI.Label(new Rect(10,75, 150, 30), "Crouch - C");
        GUI.Label(new Rect(10,100, 150, 30), "Grenade - G");
        GUI.Label(new Rect(10,125, 150, 30), "Xray - X");
        GUI.Label(new Rect(10,150, 150, 30), "Shoot - LClick");
        GUI.Label(new Rect(10,175, 150, 30), "Aim - RClick");
        GUI.Label(new Rect(10,200, 150, 30), "Reload - R");


        GUILayout.EndArea();
    }
}

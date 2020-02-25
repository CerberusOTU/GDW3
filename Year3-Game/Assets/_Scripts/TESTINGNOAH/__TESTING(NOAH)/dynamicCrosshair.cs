using UnityEngine;
using System.Collections;
using UnityEditor;

public class dynamicCrosshair : MonoBehaviour
{
    playerInputManager inputManager;
    playerCharController charController;

    [Header("Crosshair Properties")]
    public float height = 20f;
    public float width = 4f;
    public float defaultSpread = 30f;
    public Color color = Color.white;
    public bool resizeable = false;
    public float resizedSpread = 90f;
    public float resizeSpeed = 30f;

    float spread;
    bool resizing = false;

    void Start()
    {
        //set spreadplayerCharController
        spread = defaultSpread;
        charController = GetComponent<playerCharController>();
        inputManager = GetComponent<playerInputManager>();

    }

    void FixedUpdate()
    {
        adjustCrosshair();
    }

    void adjustCrosshair()
    {
        //for demonstration purposes
        if (charController.currentSpeed > charController.speed) { resizing = true; } else { resizing = false; }
        
        if (resizeable)
        {
            if (resizing)
            {
                spread = Mathf.Lerp(spread, resizedSpread, resizeSpeed * Time.fixedDeltaTime);
            }
            else
            {
                spread = Mathf.Lerp(spread, defaultSpread, resizeSpeed * Time.fixedDeltaTime);
            }
            spread = Mathf.Clamp(spread, defaultSpread, resizedSpread);
        }
    }

    void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        //up rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) + spread / 2, width, height), texture);

        //down rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) - spread / 2, width, height), texture);

        //left rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) + spread / 2, Screen.height / 2 - width / 2, height, width), texture);

        //right rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) - spread / 2, Screen.height / 2 - width / 2, height, width), texture);
    }

    public void SetRisizing(bool state)
    {
        resizing = state;
    }
}

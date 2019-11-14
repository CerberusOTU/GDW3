using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class Controller : MonoBehaviour
{
    public bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    public GamePadState state;
    public GamePadState prevState;

    float count = 0;
    public bool buttonDown;

    // Use this for initialization
    void Start()
    {
        // No need to initialize anything for the plugin
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 2; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        //prevState = state;
       // state = GamePad.GetState(playerIndex);
    }

   public bool getDown(ButtonState button)
    {
        if(button == ButtonState.Pressed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator buttonDelay()
    {
        buttonDown = true;
        yield return 0.2f;
        buttonDown = false;
    }

}

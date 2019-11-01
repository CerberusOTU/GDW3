using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Manager : MonoBehaviour
{
    private Ingame_GUI tutorialGUI;
    // Game Achievement Triggers //
    public GameObject MovementTrigger; //WASD
    public GameObject JumpTrigger; //Space
    public GameObject CrouchTrigger; //CTRL
    public GameObject PickUpTrigger; //"E"
    public GameObject ShootingTrigger; // LClick, RClick
    public GameObject XRayTrigger; //"F"
    public Texture2D isCompleteTex;
    public Texture2D notCompleteTex;

    // Player Components //
    private Rigidbody playerRB;
    private CapsuleCollider playerCC; 

    void Start()
    {
        tutorialGUI = GameObject.FindObjectOfType<Ingame_GUI>();
        playerRB = this.GetComponent<Rigidbody>();
        playerCC = this.GetComponent<CapsuleCollider>();
        for(int i =0; i < 7; i++)
        {
            tutorialGUI.crossList[i] = notCompleteTex;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Check Player movement completion

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "isUnlocked")
        {
            if (collider.gameObject == MovementTrigger)
            {
                Debug.Log("Movement Complete");
                Notify("MOVEMENT_COMPLETE");
                tutorialGUI.crossList[0] = isCompleteTex;
            }
            else if(collider.gameObject == JumpTrigger)
            {
                Debug.Log("Jump Complete");
                Notify("JUMP_COMPLETE");
            }
            else if(collider.gameObject == CrouchTrigger)
            {       
                Debug.Log("Crouch Complete");
                Notify("CROUCH_COMPLETE");  
            }
            collider.gameObject.tag = "isUnlocked";
        }
    }

    void Notify(string _achievement)
    {
        switch (_achievement)
        {
            case "MOVEMENT_COMPLETE":
                tutorialGUI.crossList[0] = isCompleteTex;
            break;
            case "JUMP_COMPLETE":
                tutorialGUI.crossList[1] = isCompleteTex;
            break;
            case "CROUCH_COMPLETE":
                tutorialGUI.crossList[2] = isCompleteTex;
            break;
        }
    }
}

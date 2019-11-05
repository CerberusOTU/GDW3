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

    // Completion Bools
    [System.NonSerialized]
    public bool b_movementComplete = false;
    [System.NonSerialized]
    public bool b_jumpComplete = false;
    [System.NonSerialized]
    public bool b_crouchComplete = false;
    [System.NonSerialized]
    public bool b_swapComplete = false;
    [System.NonSerialized]
    public bool b_shootingComplete = false;
    [System.NonSerialized]
    public bool b_grenadeComplete = false;
    [System.NonSerialized]
    public bool b_xrayComplete = false;


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

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "isUnlocked")
        {
            if (collider.gameObject == MovementTrigger)
            {
                Debug.Log("Movement Complete");
                Notify("MOVEMENT_COMPLETE");
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

    public void Notify(string _achievement)
    {
        switch (_achievement)
        {
            case "MOVEMENT_COMPLETE":
                tutorialGUI.crossList[0] = isCompleteTex;
                b_movementComplete = true;
            break;
            case "JUMP_COMPLETE":
                tutorialGUI.crossList[1] = isCompleteTex;
                b_jumpComplete = true;
            break;
            case "CROUCH_COMPLETE":
                tutorialGUI.crossList[2] = isCompleteTex;
                b_crouchComplete = true;
            break;
            case "SWAP_COMPLETE":
                tutorialGUI.crossList[3] = isCompleteTex;
                b_swapComplete = true;
            break;
            case "SHOOTING_COMPLETE":
                tutorialGUI.crossList[4] = isCompleteTex;
                b_shootingComplete = true;
            break;
            case "GRENADE_COMPLETE":
                tutorialGUI.crossList[5] = isCompleteTex;
                b_grenadeComplete = true;
            break;
            case "XRAY_COMPLETE":
                tutorialGUI.crossList[6] = isCompleteTex;
                b_xrayComplete = true;
            break;
            
        }
    }
}

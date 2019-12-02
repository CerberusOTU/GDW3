using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tutorial_Manager : MonoBehaviour
{
    private Ingame_GUI tutorialGUI;
    // Variables
    private int count = 0;
    private int tasksCount = 8;
    public int TargetCount = 0;
    public bool Target1 = false;
    public bool Target2 = false;
    public bool Target3 = false;

    public int tutorialComplete = 0;

    // Game Achievement Triggers //
    public GameObject MovementTrigger; //WASD
    public GameObject JumpTrigger; //Space
    public GameObject CrouchTrigger; //CTRL / C
    public GameObject ShootingTrigger; // LClick, RClick
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
    public bool b_reloadComplete = false;
    [System.NonSerialized]
    public bool b_grenadeComplete = false;
    [System.NonSerialized]
    public bool b_xrayComplete = false;

    public bool b_tutorialComplete;


    RaycastHit checkTask;
    public Text HintText;
    public Text CompleteText;


    // Player Components //
    private Rigidbody playerRB;
    private CapsuleCollider playerCC;
    public Camera cam;

    void Start()
    {
        
        ///////////////COMPLETE/////////////// IGNORE TUTORIAL
        // Completion Bools
        if (b_tutorialComplete)
        {
            b_movementComplete = true;
            b_jumpComplete = true;
            b_crouchComplete = true;
            b_swapComplete = true;
            b_shootingComplete = true;
            b_reloadComplete = true;
            b_grenadeComplete = true;
            b_xrayComplete = true;
            b_tutorialComplete = true;
        }else if(!b_tutorialComplete){
        

            tutorialGUI = GameObject.FindObjectOfType<Ingame_GUI>();
            playerRB = this.GetComponent<Rigidbody>();
            playerCC = this.GetComponent<CapsuleCollider>();
            for(int i = 0; i < tasksCount; i++)
            {
                tutorialGUI.crossList.Add(notCompleteTex);
            }
            CompleteText.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!b_tutorialComplete){
            HintText.enabled = false;
            //Give Hint
            GiveHint();
        }

        if (tutorialComplete == 2)
        {
            SceneManager.LoadScene("Menu");
        }
    }


    void GiveHint()
    {
        checkTask = new RaycastHit();

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out checkTask, 5f))
        {
            if (checkTask.collider.tag == "TutHint")
            {
                if (checkTask.collider.name == "XrayTester") //Find Partner
                {
                    HintText.enabled = true;
                    HintText.text = "Hold X to see your partner!";
                }
                else if (checkTask.collider.name == "Cube (3)")
                {
                    HintText.enabled = true;
                    HintText.text = "Press Space to jump on red box!";
                }
                else if (checkTask.collider.name == "Cube (4)" || checkTask.collider.name == "Cube (8)")
                {
                    HintText.enabled = true;
                    HintText.text = "Right-Click to aim. Left-Click to shoot!";
                }
            }
        }
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
        count++;
        switch (_achievement)
        {
            case "MOVEMENT_COMPLETE":
                Debug.Log("Move Complete");
                tutorialGUI.crossList[0] = isCompleteTex;
                b_movementComplete = true;
            break;
            case "JUMP_COMPLETE":
                Debug.Log("Jump Complete");
                tutorialGUI.crossList[1] = isCompleteTex;
                b_jumpComplete = true;
            break;
            case "CROUCH_COMPLETE":
                Debug.Log("Crouch Complete");
                tutorialGUI.crossList[2] = isCompleteTex;
                b_crouchComplete = true;
            break;
            case "SWAP_COMPLETE":
                Debug.Log("Swap Complete");
                tutorialGUI.crossList[3] = isCompleteTex;
                b_swapComplete = true;
            break;
            case "SHOOTING_COMPLETE":
                Debug.Log("Shooting Complete");
                tutorialGUI.crossList[4] = isCompleteTex;
                b_shootingComplete = true;
            break;
            case "RELOAD_COMPLETE":
                Debug.Log("Reload Complete");
                tutorialGUI.crossList[5] = isCompleteTex;
                b_reloadComplete = true;
            break;
            case "GRENADE_COMPLETE":
                Debug.Log("Grenade Complete");
                tutorialGUI.crossList[6] = isCompleteTex;
                b_grenadeComplete = true;
            break;
            case "XRAY_COMPLETE":
                Debug.Log("Xray Complete");
                tutorialGUI.crossList[7] = isCompleteTex;
                b_xrayComplete = true;
            break;      
        }

        if (count == tasksCount)
        {
            b_tutorialComplete = true;
            CompleteText.enabled = true;
            tutorialComplete += 1;
        }
    }

}

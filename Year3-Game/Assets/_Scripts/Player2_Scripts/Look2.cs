using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look2 : MonoBehaviour
{
    Controller controller;
    public static bool cursorLocked = true;
    public Transform player;
    public Transform cams;
    public Transform weapon;
    public float xSens;
    public float ySens;

    private float adjustY;
    private float adjustX;
    public float maxAngle;
    float inputY;
    float inputX;

    private Quaternion camCenter;
    void Start()
    {
        camCenter = cams.localRotation; //set rotation origin for cameras to camCenter

       controller = GameObject.FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        SetY();
        SetX();
        CursorLock();
    }

    void SetY()
    {
        
        if(Input.GetMouseButton(1) || controller.state2.Triggers.Left == 1)
        {
            adjustY = ySens / 4.5f;
        }
        else
        {
            adjustY = ySens;
        }
        
        if(controller.state2.IsConnected)
        {
            inputY = controller.state2.ThumbSticks.Right.Y * adjustY * Time.deltaTime;
        }
        else
        {
            inputY = Input.GetAxis("Mouse Y") * adjustY * Time.deltaTime;
        }

        Quaternion adj = Quaternion.AngleAxis(inputY, -Vector3.right);
        Quaternion delta = cams.localRotation * adj;

        if(Quaternion.Angle(camCenter, delta) < maxAngle)
        {
            cams.localRotation = delta;
        } 
        else if(cams.localRotation.eulerAngles.x < (360-maxAngle) && cams.localRotation.eulerAngles.x > maxAngle)
        {
            cams.localRotation = Quaternion.Slerp(cams.localRotation, Quaternion.Euler(maxAngle, 0, 0), Time.deltaTime * 4f);
        }
    }

     void SetX()
    {
        if(Input.GetMouseButton(1) || controller.state2.Triggers.Left == 1)
        {
            adjustX = xSens / 4.5f;
        }
        else
        {
            adjustX = xSens;
        }

        
        if(controller.state2.IsConnected)
        {
            inputX = controller.state2.ThumbSticks.Right.X * adjustX * Time.deltaTime;
        }
        else
        {
            inputX = Input.GetAxis("Mouse X") * adjustX * Time.deltaTime;
        }


        Quaternion adj = Quaternion.AngleAxis(inputX, Vector3.up);
        Quaternion delta = player.localRotation * adj;
        player.localRotation = delta;
    }

    void CursorLock()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;               
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;               
            }     
        }
    }
}

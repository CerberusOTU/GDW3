using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour
{

    playerInputManager inputManager;

    /*=====Private Variables=====*/
    //Float
    private float xRotation = 0f;
    //Camera
    private Camera fpsCamera;

    // Start is called before the first frame update
    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        inputManager = GetComponent<playerInputManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovement();
    }

    void mouseMovement()
    {
        float mouseX = inputManager.GetLookInputsHorizontal() * Time.deltaTime;
        float mouseY = inputManager.GetLookInputsVertical() * Time.deltaTime;

        if (inputManager.GetInvertYAxis())
        {
            xRotation += mouseY; //Vertical
        }
        else
        {
            xRotation -= mouseY; //Vertical
        }

        xRotation = Mathf.Clamp(xRotation, -89f, 89f); //Clamp Vertical

        fpsCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Rotate Left/Right
        transform.Rotate(Vector3.up * mouseX);
    }
}

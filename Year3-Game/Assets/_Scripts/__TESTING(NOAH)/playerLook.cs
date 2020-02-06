using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour
{

    [Header("Mouse Options")]
    [Range(1.0f, 15.0f)] [SerializeField] private float mouseSensitivity = 5.0f;
    [SerializeField] private bool InvertYAxis = false;

    /*=====Private Variables=====*/
    //Float
    private float xRotation = 0f;
    //Camera
    private Camera fpsCamera;

    // Start is called before the first frame update
    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovement();
    }

    void mouseMovement()
    {
        float mouseX = Input.GetAxisRaw(inputConstants.c_MouseAxisNameHorizontal) * mouseSensitivity * 10f * Time.deltaTime;
        float mouseY = Input.GetAxisRaw(inputConstants.c_MouseAxisNameVertical) * mouseSensitivity * 10f * Time.deltaTime;

        if (InvertYAxis)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        fpsCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    playerInputManager inputManager;

    [Header("Sway Properties")]
    [SerializeField]private float intensity;
    [SerializeField]private float smooth;

    private Quaternion origRot;

    private void Start()
    {
        inputManager = GetComponentInParent<playerInputManager>();
        origRot = transform.localRotation;
    }

    private void Update()
    {
       UpdateSway();
    }

    private void UpdateSway()
    {
        if (inputManager.GetAimInputHeld())
        {
            intensity = 0;
        }
        else
            intensity = 1;

        //controls
        float xMouse = inputManager.GetLookInputsHorizontal();
        float yMouse = inputManager.GetLookInputsVertical();

        //calculate target Rotation
        Quaternion xAdjustRot = Quaternion.AngleAxis(-intensity * xMouse, Vector3.up);
        Quaternion yAdjustRot = Quaternion.AngleAxis(intensity * yMouse, Vector3.right);

        Quaternion targetRot = origRot * xAdjustRot * yAdjustRot;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smooth);

    }
}

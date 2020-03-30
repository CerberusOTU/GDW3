using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine;
public class XRay4 : MonoBehaviour
{
    private Controller controller;
    public Camera cam;
    public GameObject partner;
    public Material baseMat;
    public Material xrayMat;

    
    //**************TUTORIAL VARIABLES**************/
    [System.NonSerialized]
    public Tutorial_Manager2 _tutManager;

     void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        partner.GetComponent<MeshRenderer> ().material = baseMat;
        controller = gameObject.GetComponent<Controller>();
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager2>();
    }

    void Update()
    {
        /* RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 100f))
        {
            if(hitInfo.collider.name == "XrayTester2")
            {
                 if (!_tutManager.b_xrayComplete)
                _tutManager.Notify("XRAY_COMPLETE");
            }
        } */

        if(controller.state4.Buttons.RightShoulder == ButtonState.Pressed)
        {
            partner.GetComponent<MeshRenderer> ().material = xrayMat;
        }
        else
        {
            partner.GetComponent<MeshRenderer> ().material = baseMat;
        }
    }
}

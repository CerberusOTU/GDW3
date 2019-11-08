using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayToggle : MonoBehaviour
{
    private Camera cam;
    public GameObject partner;
    public Material baseMat;
    public Material xrayMat;
    
    //**************TUTORIAL VARIABLES**************/
    [System.NonSerialized]
    public Tutorial_Manager _tutManager;

     void Start()
    {
        cam = Camera.main;
        //Fetch the Material from the Renderer of the GameObject
        partner.GetComponent<MeshRenderer> ().material = baseMat;
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager>();
    }

    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 100f))
        {
            if(hitInfo.collider.name == "XrayTester")
            {
                 if (!_tutManager.b_xrayComplete)
                _tutManager.Notify("XRAY_COMPLETE");
            }
        }

        if(Input.GetKey(KeyCode.X))
        {
            partner.GetComponent<MeshRenderer> ().material = xrayMat;
            // Tutorial completion check
        }
        else
        {
            partner.GetComponent<MeshRenderer> ().material = baseMat;
        }
    }
}

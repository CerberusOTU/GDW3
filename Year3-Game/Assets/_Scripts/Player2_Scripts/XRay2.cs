using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class XRay2 : MonoBehaviour
{
    
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
        
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager2>();
    }

    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 100f))
        {
            if(hitInfo.collider.name == "XrayTester2")
            {
                 if (!_tutManager.b_xrayComplete)
                _tutManager.Notify("XRAY_COMPLETE");
            }
        }

        if(Input.GetButton("XRay2"))
        {
            partner.GetComponent<MeshRenderer> ().material = xrayMat;
        }
        else
        {
            partner.GetComponent<MeshRenderer> ().material = baseMat;
        }
    }
}

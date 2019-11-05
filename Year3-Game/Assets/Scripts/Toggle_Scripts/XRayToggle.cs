using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayToggle : MonoBehaviour
{
    public GameObject partner;
    public Material baseMat;
    public Material xrayMat;
    
    //**************TUTORIAL VARIABLES**************/
    [System.NonSerialized]
    public Tutorial_Manager _tutManager;

     void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        partner.GetComponent<MeshRenderer> ().material = baseMat;
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            partner.GetComponent<MeshRenderer> ().material = xrayMat;
            // Tutorial completion check
            if (!_tutManager.b_xrayComplete)
                _tutManager.Notify("XRAY_COMPLETE");
        }
        else
        {
            partner.GetComponent<MeshRenderer> ().material = baseMat;
        }
    }
}

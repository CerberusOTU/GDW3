using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class XRay2 : MonoBehaviour
{
    public GameObject partner;
    public Material baseMat;
    public Material xrayMat;

     void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        partner.GetComponent<MeshRenderer> ().material = baseMat;
    }

    void Update()
    {
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

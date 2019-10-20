using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSilhouette : MonoBehaviour
{
 public Material[] materials;
 private MeshRenderer meshRenderer;
 
 void Start () {
 
           meshRenderer = GetComponent<MeshRenderer>();
           meshRenderer.material = materials[0];
 }
 
 void Update () {
 
           if (Input.GetKeyDown(KeyCode.X)){
               
                       meshRenderer.material = materials[1];
           }
           if (Input.GetKeyUp(KeyCode.X)){
               
                       meshRenderer.material = materials[0];
           }
 }
}

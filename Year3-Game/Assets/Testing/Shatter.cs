using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject replace;
    private GameObject GO;
    public bool shatterThis = false;

    void Update()
    {
        if(shatterThis == true)
        {
            GO = GameObject.Instantiate(replace, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
    
}

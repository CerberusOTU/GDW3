using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSwitcher : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y > -12)
        {
            float step = speed * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);    
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y < 0)
        {
            float step = speed * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);    
        }
    }
}

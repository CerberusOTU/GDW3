using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshSwitcher : MonoBehaviour
{
    public float speed;
    public Text Gun;
    private float selection = 1.0f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y > -12)
        {
            float step = speed * Time.deltaTime;
            
            transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);

            if (selection < 5.0f)
                selection += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y < 0)
        {
            float step = speed * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            if (selection > 1.0f)
                selection -= 1.0f;
        }

        if (selection == 1.0f)
            Gun.text = "Tommy Gun";
        if (selection == 2.0f)
            Gun.text = "Remington";
        if (selection == 3.0f)
            Gun.text = "Winchester";
        if (selection == 4.0f)
            Gun.text = "MP40";
        if (selection == 5.0f)
            Gun.text = "M1911";
    }
}

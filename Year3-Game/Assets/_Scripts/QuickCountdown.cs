using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickCountdown : MonoBehaviour
{

    float timer;

    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0.1f)
        {
            timer += Time.deltaTime;
            GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = false;
        }
        else
        {
            GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = true;
            Destroy(this);
        }
    }
}

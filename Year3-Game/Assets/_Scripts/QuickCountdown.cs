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
            GameObject.Find("Player").GetComponent<playerWeaponManager>().enabled = false;
        }
        else
        {
            GameObject.Find("Player").GetComponent<playerWeaponManager>().enabled = true;
            Destroy(this);
        }
    }
}

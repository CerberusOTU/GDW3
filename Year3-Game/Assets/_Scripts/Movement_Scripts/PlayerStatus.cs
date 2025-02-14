﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
   ///UI///
    public Text HealthText;
    public Image Health;
    public Image DamageOverlay;
    public Image DamageFlash;
    public float PlayerHealth = 1f;

    Vector3 temp;
    
    [SerializeField]
    public bool tookDamage = false;

    ///////////////////

    void Start()
    {
    
    }

    void Update()
    {
        var tempColor = DamageOverlay.color;
        tempColor.a = -(PlayerHealth) * 0.01f + 1;
        var tempColor2 = DamageFlash.color; 

        if (tookDamage == true && PlayerHealth > 10)
        {
            //PlayerHealth -= 10;
            tempColor2.a = 0.5f;
            tookDamage = false;
        }
        if (Input.GetKey(KeyCode.B) && PlayerHealth < 100)
        {   
            PlayerHealth += 1;
        }
      
            HealthText.text = (PlayerHealth).ToString();
        //Health Bar Transform
        temp = transform.localScale;
        temp.x = 1f;
        temp.y = 1f;
        Health.transform.localScale = temp;
        tempColor2.a -= 0.02f;

        DamageOverlay.color = tempColor;
        DamageFlash.color = tempColor2;
    }

}

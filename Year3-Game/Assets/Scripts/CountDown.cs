using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text txt;
    public Text txt2;

    public Canvas player1;
    public Canvas player2;

    public Canvas crossHair1;
    public Canvas crossHair2;


    float timer = 0;
    void Update()
    {  
        if (timer <= 3)
        {
            timer += Time.deltaTime;
            player1.enabled = false;
            player2.enabled = false;
            crossHair1.enabled = false;
            crossHair2.enabled = false;
            GameObject.Find("Player").GetComponent<Motion>().enabled = false;
            GameObject.Find("Player2").GetComponent<Movement2>().enabled = false;

            GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = false;
            if(timer >= 1 && timer < 2)
            {
                txt.text = "2";
                txt2.text = "2";
            }
             else if(timer >= 2 && timer < 3)
            {
                txt.text = "1";
                txt2.text = "1";
            }
            else if(timer >= 3 && timer <= 4)
            {
                txt.enabled = false;
                txt2.enabled = false;

                player1.enabled = true;
                player2.enabled = true;

                crossHair1.enabled = true;
                crossHair2.enabled = true;

                GameObject.Find("Player").GetComponent<Motion>().enabled = true;
                GameObject.Find("Player2").GetComponent<Movement2>().enabled = true;

                GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
                GameObject.Find("Player2").GetComponent<Weapon2>().enabled = true;
            } 

        }
    }
}

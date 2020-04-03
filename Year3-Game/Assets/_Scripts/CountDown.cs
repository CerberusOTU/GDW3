using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text txt;
    public Text txt2;
    public Text txt3;
    public Text txt4;

    public Canvas player1;
    public Canvas player2;

    public Canvas player3;
    public Canvas player4;

    public Canvas crossHair1;
    public Canvas crossHair2;
    public Canvas crossHair3;
    public Canvas crossHair4;
    bool check = true;

    void Start()
    {
        GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
        GameObject.Find("Player2").GetComponent<Weapon2>().enabled = false;
        GameObject.Find("Player3").GetComponent<Weapon3>().enabled = false;
        GameObject.Find("Player4").GetComponent<Weapon4>().enabled = false;

        GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
        GameObject.Find("Player2").GetComponent<Weapon2>().enabled = true;
        GameObject.Find("Player3").GetComponent<Weapon3>().enabled = true;
        GameObject.Find("Player4").GetComponent<Weapon4>().enabled = true;
    }

    float timer = 0;
    void Update()
    {  
        if (timer <= 3)
        {
            timer += Time.deltaTime;
            player1.enabled = false;
            player2.enabled = false;
            player3.enabled = false;
            player4.enabled = false;
            crossHair1.enabled = false;
            crossHair2.enabled = false;
            crossHair3.enabled = false;
            crossHair4.enabled = false;
            GameObject.Find("Player").GetComponent<Motion>().enabled = false;
            GameObject.Find("Player2").GetComponent<Movement2>().enabled = false;
            GameObject.Find("Player3").GetComponent<Movement3>().enabled = false;
            GameObject.Find("Player4").GetComponent<Movement4>().enabled = false;
            GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = false;
            GameObject.Find("Player3").GetComponent<Weapon3>().enabled = false;
            GameObject.Find("Player4").GetComponent<Weapon4>().enabled = false;

            if (check == true)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/General/Ticking", GameObject.Find("Player").GetComponent<Weapon>().transform.position);
                check = false;
            }

            if (timer >= 1 && timer < 2)
            {
                txt.text = "2";
                txt2.text = "2";
                txt3.text = "2";
                txt4.text = "2";
            }
             else if(timer >= 2 && timer < 3)
            {
                txt.text = "1";
                txt2.text = "1";
                txt3.text = "1";
                txt4.text = "1";
            }
            
            else if(timer >= 3 && timer <= 4)
            {
                txt.enabled = false;
                txt2.enabled = false;
                txt3.enabled = false;
                txt4.enabled = false;

                player1.enabled = true;
                player2.enabled = true;
                player3.enabled = true;
                player4.enabled = true;

                crossHair1.enabled = true;
                crossHair2.enabled = true;
                crossHair3.enabled = true;
                crossHair4.enabled = true;

                GameObject.Find("Player").GetComponent<Motion>().enabled = true;
                GameObject.Find("Player2").GetComponent<Movement2>().enabled = true;
                GameObject.Find("Player3").GetComponent<Movement3>().enabled = true;
                GameObject.Find("Player4").GetComponent<Movement4>().enabled = true;
                GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
                GameObject.Find("Player2").GetComponent<Weapon2>().enabled = true;
                GameObject.Find("Player3").GetComponent<Weapon3>().enabled = true;
                GameObject.Find("Player4").GetComponent<Weapon4>().enabled = true;
            } 

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int Kills = 0;
    public Text killScore;
    public Text killScore2;

    public Text condition;

    public Canvas player1;
    public Canvas player2;
    public Canvas player3;
    public Canvas player4;

    public Canvas crossHair1;
    public Canvas crossHair2;
    public Canvas crossHair3;
    public Canvas crossHair4;

    private Weapon weaponScript;
    private Weapon2 weaponScript2;
    private Weapon3 weaponScript3;
    private Weapon4 weaponScript4;

    public static Text member1;
    public static Text member2;

    int safety = 0;
    float timer;

    void Start()
    {
        condition.enabled = false;

        weaponScript = GameObject.FindObjectOfType<Weapon>();
        weaponScript2 = GameObject.FindObjectOfType<Weapon2>();      
        weaponScript3 = GameObject.FindObjectOfType<Weapon3>();
        weaponScript4 = GameObject.FindObjectOfType<Weapon4>();
        Debug.Log("check start");
        member1.text = "Testing1";
        member2.text = "Testing2";
    }

    void Update()
    {
        killScore.text = Kills.ToString();
        killScore2.text = Kills.ToString();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Kills += 1;
        }

        if (Kills == 5 && safety < 1)
        {
            safety += 1;
            this.gameObject.AddComponent<SlowTime>();
        }

      
        Debug.Log("checkname " + weaponScript.PlayerName.ToString());
        Debug.Log("checkname " + weaponScript2.PlayerName.ToString());
           Debug.Log("checkmember " + member1.text.ToString());
           Debug.Log("checkmember " + member2.text.ToString());

        if (Kills == 5)
        {
            member1.text = weaponScript.PlayerName.ToString();
            member2.text = weaponScript2.PlayerName.ToString();

            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;
            player3.enabled = false;
            player4.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;
            crossHair3.enabled = false;
            crossHair4.enabled = false;

            condition.enabled = true;

            timer += Time.deltaTime;
            if (timer >= 1)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("afterGame");
            }
        }
    }
}
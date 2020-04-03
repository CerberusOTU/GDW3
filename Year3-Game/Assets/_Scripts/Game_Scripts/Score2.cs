using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score2 : MonoBehaviour
{
    public int Kills = 0;
    public Text killScore;
    public Text killScore2;
     public Text killScore3;
    public Text  killScore4;

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

    int safety;
    float timer;

    void Start()
    {
        condition.enabled = false;

        weaponScript = GetComponent<Weapon>();
        weaponScript2 = GetComponent<Weapon2>();
        weaponScript3 = GetComponent<Weapon3>();
        weaponScript4 = GetComponent<Weapon4>();
    }
    void Update()
    {
        killScore.text = Kills.ToString();
        killScore2.text = Kills.ToString();
        
        killScore3.text = Kills.ToString();
        killScore4.text = Kills.ToString();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Kills += 1;
        }

        if (Kills == 5 && safety < 1)
        {
            safety += 1;
            this.gameObject.AddComponent<SlowTime>();
        }

        if(Kills == 5)
        {
            Destroy(GetComponent<Target>());

            player1.enabled = false;
            player2.enabled = false;
            player3.enabled = false;
            player4.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;

            condition.enabled = true;
            //GameData.member1.text = weaponScript3.PlayerName.ToString();
           // GameData.member2.text = weaponScript4.PlayerName.ToString();
            timer += Time.deltaTime;

            if(timer >= 1)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class Score : MonoBehaviour
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

    public Canvas mainHud;
    public Camera mainCamera;
    public Text winners;
    public PlayableDirector timeline;

    bool check;

    int safety = 0;
    float timer;

    void Start()
    {
        check = false;
        condition.enabled = false;
        mainHud.enabled = false;
        mainCamera.enabled = false;

        weaponScript = GameObject.FindObjectOfType<Weapon>();
        weaponScript2 = GameObject.FindObjectOfType<Weapon2>();      
        weaponScript3 = GameObject.FindObjectOfType<Weapon3>();
        weaponScript4 = GameObject.FindObjectOfType<Weapon4>();

    }

    void Update()
    {
        killScore.text = Kills.ToString();
        killScore2.text = Kills.ToString();
        killScore3.text = Kills.ToString();
        killScore4.text = Kills.ToString();
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            Kills += 1;
        }

        if (Kills == 10 && safety < 1)
        {
            safety += 1;
            this.gameObject.AddComponent<SlowTime>();
            winners.text = weaponScript.PlayerName.ToString() + " and " + weaponScript2.PlayerName.ToString() + " win!";
        }

      
     

        if (Kills == 10)
        {


            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;
            player3.enabled = false;
            player4.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;
            crossHair3.enabled = false;
            crossHair4.enabled = false;

            weaponScript.enabled = false;
            weaponScript2.enabled = false;
            weaponScript3.enabled = false;
            weaponScript4.enabled = false;

            condition.enabled = true;
          
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
                mainHud.enabled = true;
                mainCamera.enabled = true;
                timeline.Play();
                
                if (check == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Thing", weaponScript.currentWeapon);

                    check = true;
                }
           if (timer >= 21)
                {
                    SceneManager.LoadScene("Menu");
                }
                //SceneManager.LoadScene("afterGame");
            }
        }
    }
}
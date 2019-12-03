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

    public Text condition;

    public Canvas player1;
    public Canvas player2;

    public Canvas crossHair1;
    public Canvas crossHair2;

    int safety;
    float timer;

    void Start()
    {
        condition.enabled = false;
    }
    void Update()
    {
        killScore.text = Kills.ToString();
        killScore2.text = Kills.ToString();

        
        if(Kills == 5 && safety < 1)
        {
            safety += 1;
            this.gameObject.AddComponent<SlowTime>();
        }

        if(Time.timeScale != 1f && Kills < 5)
        {
            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;

            condition.enabled = true;
            condition.text = "YOU LOSE";
        }

        if(Kills == 5)
        {
            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;

            condition.enabled = true;
            condition.text = "YOU WIN";
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

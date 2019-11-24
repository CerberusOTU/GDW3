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

    public Canvas crossHair1;
    public Canvas crossHair2;

    int safety = 0;
    float timer;

    void Start()
    {
        condition.enabled = false;
    }

    void Update()
    {
        killScore.text = Kills.ToString();
        killScore2.text = Kills.ToString();

        if(Kills == 1 && safety < 1)
        {
            safety += 1;
            this.gameObject.AddComponent<SlowTime>();
        }

        if(Time.timeScale != 1f && Kills < 1)
        {
            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;

            condition.enabled = true;
            condition.text = "YOU LOSE";
        }

        if(Kills == 1)
        {
            Destroy(GetComponent<Target>());
            player1.enabled = false;
            player2.enabled = false;

            crossHair1.enabled = false;
            crossHair2.enabled = false;

            condition.enabled = true;
            condition.text = "YOU WIN";

            timer += Time.deltaTime;
            if(timer >= 2.5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;  
            }
        }
    }
}

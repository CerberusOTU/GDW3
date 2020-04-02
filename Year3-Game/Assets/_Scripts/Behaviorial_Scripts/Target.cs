using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject defaultSpawn;
    public Transform[] playerSpawn;
    private float[] dist = new float[18];
    private List<Transform> SpawnPos = new List<Transform>();

    public float health = 50f;
    PlayerStatus healthUI;
    PlayerStatus2 healthUI2;

    PlayerStatus3 healthUI3;
    PlayerStatus4 healthUI4;

    public Image Grenade1;
    public Image Grenade2;
    public Image Player2Grenade1;
    public Image Player2Grenade2;


    public Image Player3Grenade1;
    public Image Player3Grenade2;
    public Image Player4Grenade1;
    public Image Player4Grenade2;

    //public Camera cam;
    //public Camera deathcam;
    //public GameObject HUD;
    
    public Text KilledEnemy;
    public Image KilledIcon;
    public RawImage Fade;

    public RawImage Banner;
    public Text KilledBy;
    public Text Respawn;

    public Image whiteBanner;
    public float maxTime = 3f;
    float timeLeft;
    bool timeStart;

    bool beatTrigger;

    Vector3 lerpPos = new Vector3(0f, 2f, 0f);
    Vector3 startPos;
    Vector3 startPos2;
    //Vector3 endPos = new Vector3(-77.02f, 0.14f, 44.21f);

    private Weapon weaponScript;
    private Weapon2 weaponScript2;
    private Weapon3 weaponScript3;
    private Weapon4 weaponScript4;

    public GameObject weapon1;

    Score score;
    Score2 score2;

    public GameObject BaseModel;
    public GameObject Ragdoll;
    GameObject go;

int Max(float[] arr)
{
     var max = arr[0];
     int location = 0;
     for (int i = 1; i < arr.Length; i++) 
     {
         if (arr[i] > max) {
             max = arr[i];
             location = i;
         }
     }
     return (location);
 
}
    void Spawn()
    {
        if(this.name == "Player")
        {
        for(int i = 0; i < playerSpawn.Length; i++)
        {
            dist[i] = Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, playerSpawn[i].position);
        }
          int index = Max(dist);

          this.gameObject.transform.position = playerSpawn[index].position;
          this.gameObject.transform.rotation = playerSpawn[index].rotation;
        }
        else if(this.name == "Player2")
        {
        for(int i = 0; i < playerSpawn.Length; i++)
        {
            dist[i] = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, playerSpawn[i].position);
        }
            int index = Max(dist);
          
          this.gameObject.transform.position = playerSpawn[index].position;
          this.gameObject.transform.rotation = playerSpawn[index].rotation;
        }
        else if(this.name == "Player3")
        {
        for(int i = 0; i < playerSpawn.Length; i++)
        {
            dist[i] = Vector3.Distance(GameObject.Find("Player4").GetComponent<Transform>().position, playerSpawn[i].position);
        }
            int index = Max(dist);
          
          this.gameObject.transform.position = playerSpawn[index].position;
          this.gameObject.transform.rotation = playerSpawn[index].rotation;
        }

        else if(this.name == "Player4")
        {
        for(int i = 0; i < playerSpawn.Length; i++)
        {
            dist[i] = Vector3.Distance(GameObject.Find("Player3").GetComponent<Transform>().position, playerSpawn[i].position);
        }
            int index = Max(dist);
          
          this.gameObject.transform.position = playerSpawn[index].position;
          this.gameObject.transform.rotation = playerSpawn[index].rotation;
        }
    }

    void Start()
    {
        timeLeft = maxTime;
        beatTrigger = false;
        Vector3 startPos = KilledEnemy.transform.localPosition;
        Vector3 startPos2 = KilledIcon.transform.localPosition;

        weaponScript = GameObject.FindObjectOfType<Weapon>();
        weaponScript2 = GameObject.FindObjectOfType<Weapon2>();
        weaponScript3 = GameObject.FindObjectOfType<Weapon3>();
        weaponScript4 = GameObject.FindObjectOfType<Weapon4>();

        healthUI = GameObject.FindObjectOfType<PlayerStatus>();
        healthUI2 = GameObject.FindObjectOfType<PlayerStatus2>();
        healthUI3 = GameObject.FindObjectOfType<PlayerStatus3>();
        healthUI4 = GameObject.FindObjectOfType<PlayerStatus4>();

        score = GameObject.FindObjectOfType<Score>();
        score2 = GameObject.FindObjectOfType<Score2>();

        KilledEnemy.canvasRenderer.SetAlpha(0f);
        KilledIcon.canvasRenderer.SetAlpha(0f);
        Fade.canvasRenderer.SetAlpha(0f);
        Banner.canvasRenderer.SetAlpha(0f);
        KilledBy.canvasRenderer.SetAlpha(0f);
        Respawn.canvasRenderer.SetAlpha(0f);
        whiteBanner.canvasRenderer.SetAlpha(0f);

    }

    void Update()
    {

        if (timeStart == true)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                whiteBanner.fillAmount = timeLeft / maxTime;
            }
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        /* if (health < 30 && beatTrigger == false)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player Effects/Heartbeat", weaponScript.currentWeapon);
            beatTrigger = true;
        }
        else if(health >= 30)
        {
            beatTrigger = false;
        } */
        if (this.gameObject.name == "Player")
        {
            healthUI.tookDamage = true;
            healthUI.PlayerHealth -= amount;
        }
        else if (this.gameObject.name == "Player2")
        {
            healthUI2.tookDamage = true;
            healthUI2.PlayerHealth -= amount;
        }
        else if (this.gameObject.name == "Player3")
        {
            healthUI3.tookDamage = true;
            healthUI3.PlayerHealth -= amount;
        }
        else if (this.gameObject.name == "Player4")
        {
            healthUI4.tookDamage = true;
            healthUI4.PlayerHealth -= amount;
        }

        if (health <= 0f && (this.gameObject.name == "Player2" || this.gameObject.name == "Player4"))
        {

            score.Kills += 1;
            KilledEnemy.text = weaponScript2.PlayerName;

            StartCoroutine(Die());
            StartCoroutine(UIElements());
        }
        else if (health <= 0f && (this.gameObject.name == "Player" || this.gameObject.name == "Player3"))
        {
            score2.Kills += 1;
            KilledBy.text = "You were killed by " + weaponScript.PlayerName;

            StartCoroutine(Die());
            StartCoroutine(UIElements());
        }
    }

    IEnumerator UIElements()
    {
        //KilledEnemy.text = weaponScript.PlayerName.ToString();

        //while (KilledEnemy.transform.localPosition != (startPos + lerpPos))
        //{
        //    KilledEnemy.transform.localPosition = Vector3.MoveTowards(startPos, startPos + lerpPos, 0.000001f * Time.deltaTime);
        //    KilledIcon.transform.localPosition = Vector3.Lerp(KilledIcon.transform.localPosition, startPos + lerpPos, Time.deltaTime);
        //    yield return null;
        //}
        KilledEnemy.CrossFadeAlpha(1f, 0.2f, false);
        KilledIcon.CrossFadeAlpha(1f, 0.2f, false);
        yield return new WaitForSeconds(1f);


        KilledEnemy.CrossFadeAlpha(0f, 0.5f, false);
        KilledIcon.CrossFadeAlpha(0f, 0.5f, false);      
    }

    IEnumerator Die()
    {
        BaseModel.SetActive(false);
        GameObject go = Instantiate(Ragdoll, this.transform.position, this.transform.rotation); 
        Destroy(go, 3f);
        //Destroy(gameObject);

        health = 100f;

        if (this.gameObject.name == "Player")
        {
            weaponScript.loadout[0] = null;
            weaponScript.cam.enabled = !weaponScript.cam.enabled;
            weaponScript.deathcam.enabled = !weaponScript.deathcam.enabled;
            weaponScript.HUD.GetComponent<Canvas>().enabled = !weaponScript.HUD.GetComponent<Canvas>().enabled;
            weaponScript.crossHair.enabled = false;
            GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
            GameObject.Find("Player").GetComponent<Motion>().enabled = false;
            weapon1.SetActive(false);

            Banner.canvasRenderer.SetAlpha(1f);
            KilledBy.canvasRenderer.SetAlpha(1f);
            Respawn.canvasRenderer.SetAlpha(1f);
            whiteBanner.canvasRenderer.SetAlpha(1f);

            timeStart = true;
           

            Fade.CrossFadeAlpha(1f, 2f, false);

            yield return new WaitForSeconds(3f);

            timeStart = false;
            timeLeft = 3f;

            Fade.canvasRenderer.SetAlpha(0f);
            weapon1.SetActive(true);
            GameObject.Find("Player").GetComponent<Motion>().enabled = true;
            GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
            weaponScript.cam.enabled = !weaponScript.cam.enabled;
            weaponScript.deathcam.enabled = !weaponScript.deathcam.enabled;
            weaponScript.HUD.GetComponent<Canvas>().enabled = !weaponScript.HUD.GetComponent<Canvas>().enabled;
            weaponScript.crossHair.enabled = true;

            Banner.canvasRenderer.SetAlpha(0f);
            KilledBy.canvasRenderer.SetAlpha(0f);
            Respawn.canvasRenderer.SetAlpha(0f);
            whiteBanner.canvasRenderer.SetAlpha(0f);

            Spawn();
            healthUI.PlayerHealth = 100;
            Grenade1.enabled = true;
            Grenade2.enabled = true;

            weaponScript.Equip(1);
            weaponScript.loadout[0] = null;

            weaponScript.grenadeAmount = 2;
            Debug.Log("Grenades: " + weaponScript.grenadeAmount);

            BaseModel.SetActive(true);  
        }
        else if(this.gameObject.name == "Player2")
        {
            weaponScript2.loadout[0] = null;
            weaponScript2.cam.enabled = !weaponScript2.cam.enabled;
            weaponScript2.deathcam.enabled = !weaponScript2.deathcam.enabled;
            weaponScript2.HUD.GetComponent<Canvas>().enabled = !weaponScript2.HUD.GetComponent<Canvas>().enabled;
            weaponScript2.crossHair.enabled = false;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = false;
            GameObject.Find("Player2").GetComponent<Movement2>().enabled = false;
            weapon1.SetActive(false);

            Banner.canvasRenderer.SetAlpha(1f);
            KilledBy.canvasRenderer.SetAlpha(1f);
            Respawn.canvasRenderer.SetAlpha(1f);
            whiteBanner.canvasRenderer.SetAlpha(1f);

            timeStart = true;

            Fade.CrossFadeAlpha(1f, 2f, false);

            yield return new WaitForSeconds(3f);

            timeStart = false;
            timeLeft = 3f;

            Fade.canvasRenderer.SetAlpha(0f);
            weapon1.SetActive(true);
            GameObject.Find("Player2").GetComponent<Movement2>().enabled = true;
            GameObject.Find("Player2").GetComponent<Weapon2>().enabled = true;
            weaponScript2.cam.enabled = !weaponScript2.cam.enabled;
            weaponScript2.deathcam.enabled = !weaponScript2.deathcam.enabled;
            weaponScript2.HUD.GetComponent<Canvas>().enabled = !weaponScript2.HUD.GetComponent<Canvas>().enabled;
            weaponScript2.crossHair.enabled = true;

            Banner.canvasRenderer.SetAlpha(0f);
            KilledBy.canvasRenderer.SetAlpha(0f);
            Respawn.canvasRenderer.SetAlpha(0f);
            whiteBanner.canvasRenderer.SetAlpha(0f);

            Spawn();
            healthUI2.PlayerHealth = 100;
            Player2Grenade1.enabled = true;
            Player2Grenade2.enabled = true;
            weaponScript2.grenadeAmount = 2;
            Debug.Log("Grenades2: " + weaponScript2.grenadeAmount);

            BaseModel.SetActive(true);  
        }
        else if(this.gameObject.name == "Player3")
        {
            weaponScript3.loadout[0] = null;
            weaponScript3.cam.enabled = !weaponScript3.cam.enabled;
            weaponScript3.deathcam.enabled = !weaponScript3.deathcam.enabled;
            weaponScript3.HUD.GetComponent<Canvas>().enabled = !weaponScript3.HUD.GetComponent<Canvas>().enabled;
            weaponScript3.crossHair.enabled = false;
            GameObject.Find("Player3").GetComponent<Weapon3>().enabled = false;
            GameObject.Find("Player3").GetComponent<Movement3>().enabled = false;
            weapon1.SetActive(false);

            Banner.canvasRenderer.SetAlpha(1f);
            KilledBy.canvasRenderer.SetAlpha(1f);
            Respawn.canvasRenderer.SetAlpha(1f);
            whiteBanner.canvasRenderer.SetAlpha(1f);

            timeStart = true;

            Fade.CrossFadeAlpha(1f, 2f, false);

            yield return new WaitForSeconds(3f);

            timeStart = false;
            timeLeft = 3f;

            Fade.canvasRenderer.SetAlpha(0f);
            weapon1.SetActive(true);
            GameObject.Find("Player3").GetComponent<Movement3>().enabled = true;
            GameObject.Find("Player3").GetComponent<Weapon3>().enabled = true;
            weaponScript3.cam.enabled = !weaponScript3.cam.enabled;
            weaponScript3.deathcam.enabled = !weaponScript3.deathcam.enabled;
            weaponScript3.HUD.GetComponent<Canvas>().enabled = !weaponScript3.HUD.GetComponent<Canvas>().enabled;
            weaponScript3.crossHair.enabled = true;

            Banner.canvasRenderer.SetAlpha(0f);
            KilledBy.canvasRenderer.SetAlpha(0f);
            Respawn.canvasRenderer.SetAlpha(0f);
            whiteBanner.canvasRenderer.SetAlpha(0f);

            Spawn();
            healthUI3.PlayerHealth = 100;
            Player3Grenade1.enabled = true;
            Player3Grenade2.enabled = true;
            weaponScript3.grenadeAmount = 2;
            Debug.Log("Grenades2: " + weaponScript3.grenadeAmount);

            BaseModel.SetActive(true);  
        }
        else if(this.gameObject.name == "Player4")
        {
            weaponScript4.loadout[0] = null;
            weaponScript4.cam.enabled = !weaponScript4.cam.enabled;
            weaponScript4.deathcam.enabled = !weaponScript4.deathcam.enabled;
            weaponScript4.HUD.GetComponent<Canvas>().enabled = !weaponScript4.HUD.GetComponent<Canvas>().enabled;
            weaponScript4.crossHair.enabled = false;
            GameObject.Find("Player4").GetComponent<Weapon4>().enabled = false;
            GameObject.Find("Player4").GetComponent<Movement4>().enabled = false;
            weapon1.SetActive(false);

            Banner.canvasRenderer.SetAlpha(1f);
            KilledBy.canvasRenderer.SetAlpha(1f);
            Respawn.canvasRenderer.SetAlpha(1f);
            whiteBanner.canvasRenderer.SetAlpha(1f);

            timeStart = true;

            Fade.CrossFadeAlpha(1f, 2f, false);

            yield return new WaitForSeconds(3f);

            timeStart = false;
            timeLeft = 3f;

            Fade.canvasRenderer.SetAlpha(0f);
            weapon1.SetActive(true);
            GameObject.Find("Player4").GetComponent<Movement4>().enabled = true;
            GameObject.Find("Player4").GetComponent<Weapon4>().enabled = true;
            weaponScript4.cam.enabled = !weaponScript4.cam.enabled;
            weaponScript4.deathcam.enabled = !weaponScript4.deathcam.enabled;
            weaponScript4.HUD.GetComponent<Canvas>().enabled = !weaponScript4.HUD.GetComponent<Canvas>().enabled;
            weaponScript4.crossHair.enabled = true;

            Banner.canvasRenderer.SetAlpha(0f);
            KilledBy.canvasRenderer.SetAlpha(0f);
            Respawn.canvasRenderer.SetAlpha(0f);
            whiteBanner.canvasRenderer.SetAlpha(0f);

            Spawn();
            healthUI4.PlayerHealth = 100;
            Player4Grenade1.enabled = true;
            Player4Grenade2.enabled = true;
            weaponScript4.grenadeAmount = 2;
            Debug.Log("Grenades2: " + weaponScript4.grenadeAmount);

            BaseModel.SetActive(true);  
        }
    }
}

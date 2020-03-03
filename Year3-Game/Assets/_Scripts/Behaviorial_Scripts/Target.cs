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

    public Image Grenade1;
    public Image Grenade2;
    public Image Player2Grenade1;
    public Image Player2Grenade2;

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

    Vector3 lerpPos = new Vector3(0f, 2f, 0f);
    Vector3 startPos;
    Vector3 startPos2;
    //Vector3 endPos = new Vector3(-77.02f, 0.14f, 44.21f);

    private Weapon weaponScript;
    private Weapon2 weaponScript2;

    public GameObject weapon1;
    public GameObject weapon2;

    private float distanceToSpawnPoint1, distanceToSpawnPoint2, distanceToSpawnPoint3, distanceToSpawnPoint4;

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

        if(this.name == "Player2")
        {
        for(int i = 0; i < playerSpawn.Length; i++)
        {
            dist[i] = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, playerSpawn[i].position);
        }
            int index = Max(dist);
          
          this.gameObject.transform.position = playerSpawn[index].position;
          this.gameObject.transform.rotation = playerSpawn[index].rotation;
        }

    }

    void Start()
    {
        timeLeft = maxTime;

        Vector3 startPos = KilledEnemy.transform.localPosition;
        Vector3 startPos2 = KilledIcon.transform.localPosition;

        weaponScript = GetComponent<Weapon>();
        weaponScript2 = GetComponent<Weapon2>();

        healthUI = GameObject.FindObjectOfType<PlayerStatus>();
        healthUI2 = GameObject.FindObjectOfType<PlayerStatus2>();

        score = GameObject.FindObjectOfType<Score>();
        score2 = GameObject.FindObjectOfType<Score2>();

        //generate respawn transforms
        for (int i = 0; i < 4; i++)
        {
            Transform temp = Instantiate(defaultSpawn.transform);
            SpawnPos.Add(temp);
            //temp.transform.parent = this.transform;
        }

        //set weapon spawn locations

        SpawnPos[0].position = new Vector3(-85.4f, 1f, 45f);
        SpawnPos[0].localRotation *= Quaternion.Euler(0f, 180f, 0f);

        SpawnPos[1].position = new Vector3(-50.7f, 1f, -1.1f);
        SpawnPos[1].localRotation *= Quaternion.Euler(0f, 270f, 0f);

        SpawnPos[2].position = new Vector3(-110.3f, 1f, 22.6f);
        SpawnPos[2].localRotation *= Quaternion.Euler(0f, 90, 0f);

        SpawnPos[3].position = new Vector3(-102.6f, 1f, -15.3f);
        SpawnPos[3].localRotation *= Quaternion.Euler(0f, 90f, 0f);


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
        if (health <= 0f && this.gameObject.name == "Player2")
        {
            score.Kills += 1;
            KilledEnemy.text = weaponScript2.PlayerName;

            StartCoroutine(Die());
            StartCoroutine(UIElements());

        }
        else if (health <= 0f && this.gameObject.name == "Player")
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
        int index = Random.Range(0, 3);
        Debug.Log(index);

        Debug.Log(SpawnPos[index].position);

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

           /*  distanceToSpawnPoint1 = Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(27f, 3.2f, 34f));
            distanceToSpawnPoint2 = Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(45f, 3.2f, 6f));
            distanceToSpawnPoint3 = Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(10f, 3.2f, -19.5f));
            distanceToSpawnPoint4 = Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(-10f, 3.2f, 5.5f));

            //Debug.Log("Spawn 1: " + distanceToSpawnPoint1);
            //Debug.Log("Spawn 2: " + distanceToSpawnPoint2);
            //Debug.Log("Spawn 3 " + distanceToSpawnPoint3);
            //Debug.Log("Spawn 4: " + distanceToSpawnPoint4);

            if (distanceToSpawnPoint1 > distanceToSpawnPoint2 && distanceToSpawnPoint1 > distanceToSpawnPoint3 && distanceToSpawnPoint1 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[0].position;
                this.gameObject.transform.localRotation = SpawnPos[0].localRotation;
            }
            else if (distanceToSpawnPoint2 > distanceToSpawnPoint1 && distanceToSpawnPoint2 > distanceToSpawnPoint3 && distanceToSpawnPoint2 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[1].position;
                this.gameObject.transform.localRotation = SpawnPos[1].localRotation;
            }
            else if (distanceToSpawnPoint3 > distanceToSpawnPoint1 && distanceToSpawnPoint3 > distanceToSpawnPoint2 && distanceToSpawnPoint3 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[2].position;
                this.gameObject.transform.localRotation = SpawnPos[2].localRotation;
            }
            else if (distanceToSpawnPoint4 > distanceToSpawnPoint1 && distanceToSpawnPoint4 > distanceToSpawnPoint2 && distanceToSpawnPoint4 > distanceToSpawnPoint3)
            {
                this.gameObject.transform.position = SpawnPos[3].position;
                this.gameObject.transform.localRotation = SpawnPos[3].localRotation;
            }
            else if (distanceToSpawnPoint2 > distanceToSpawnPoint1 && distanceToSpawnPoint2 > distanceToSpawnPoint3 && distanceToSpawnPoint2 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[1].position;
                this.gameObject.transform.localRotation = SpawnPos[1].localRotation;
            }
            else if (distanceToSpawnPoint3 > distanceToSpawnPoint1 && distanceToSpawnPoint3 > distanceToSpawnPoint2 && distanceToSpawnPoint3 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[2].position;
                this.gameObject.transform.localRotation = SpawnPos[2].localRotation;
            }
            else if (distanceToSpawnPoint4 > distanceToSpawnPoint1 && distanceToSpawnPoint4 > distanceToSpawnPoint2 && distanceToSpawnPoint4 > distanceToSpawnPoint3)
            {
                this.gameObject.transform.position = SpawnPos[3].position;
                this.gameObject.transform.localRotation = SpawnPos[3].localRotation;
            } */

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
        else
        {

           /*  distanceToSpawnPoint1 = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, SpawnPos[0].position);
            distanceToSpawnPoint2 = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, SpawnPos[1].position);
            distanceToSpawnPoint3 = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, SpawnPos[2].position);
            distanceToSpawnPoint4 = Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, SpawnPos[3].position);

            //Debug.Log("Spawn 1: " + distanceToSpawnPoint1);
            //Debug.Log("Spawn 2: " + distanceToSpawnPoint2);
            //Debug.Log("Spawn 3 " + distanceToSpawnPoint3);
            //Debug.Log("Spawn 4: " + distanceToSpawnPoint4);

            if (distanceToSpawnPoint1 > distanceToSpawnPoint2 && distanceToSpawnPoint1 > distanceToSpawnPoint3 && distanceToSpawnPoint1 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[0].position;
                this.gameObject.transform.localRotation = SpawnPos[0].localRotation;
            }
            else if (distanceToSpawnPoint2 > distanceToSpawnPoint1 && distanceToSpawnPoint2 > distanceToSpawnPoint3 && distanceToSpawnPoint2 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[1].position;
                this.gameObject.transform.localRotation = SpawnPos[1].localRotation;
            }
            else if (distanceToSpawnPoint3 > distanceToSpawnPoint1 && distanceToSpawnPoint3 > distanceToSpawnPoint2 && distanceToSpawnPoint3 > distanceToSpawnPoint4)
            {
                this.gameObject.transform.position = SpawnPos[2].position;
                this.gameObject.transform.localRotation = SpawnPos[2].localRotation;
            }
            else if (distanceToSpawnPoint4 > distanceToSpawnPoint1 && distanceToSpawnPoint4 > distanceToSpawnPoint2 && distanceToSpawnPoint4 > distanceToSpawnPoint3)
            {
                this.gameObject.transform.position = SpawnPos[3].position;
                this.gameObject.transform.localRotation = SpawnPos[3].localRotation;
            } */
            Spawn();
            healthUI2.PlayerHealth = 100;
            Player2Grenade1.enabled = true;
            Player2Grenade2.enabled = true;
            weaponScript2.grenadeAmount = 2;
            Debug.Log("Grenades2: " + weaponScript2.grenadeAmount);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject defaultSpawn;
    private List<Transform> SpawnPos = new List<Transform>();

    public float health = 50f;
    PlayerStatus healthUI;
    PlayerStatus2 healthUI2;

    public Image Grenade1;
    public Image Grenade2;
    public Image Player2Grenade1;
    public Image Player2Grenade2;

    private Weapon weaponScript;

    private float distanceToSpawnPoint1, distanceToSpawnPoint2, distanceToSpawnPoint3, distanceToSpawnPoint4;

    Score score;
    Score2 score2;
    void Start()
    {
        weaponScript = GetComponent<Weapon>();

        healthUI = GameObject.FindObjectOfType<PlayerStatus>();
        healthUI2 = GameObject.FindObjectOfType<PlayerStatus2>();

        score = GameObject.FindObjectOfType<Score>();
        score2 = GameObject.FindObjectOfType<Score2>();

        //generate respawn transforms
         for(int i = 0; i < 4; i++)
         {
             Transform temp = Instantiate(defaultSpawn.transform); 
             SpawnPos.Add(temp);
             //temp.transform.parent = this.transform;
         }

         //set weapon spawn locations

    SpawnPos[0].position = new Vector3(27f, 3.2f, 34f);
    SpawnPos[0].localRotation *= Quaternion.Euler(0f,180f,0f);

    SpawnPos[1].position = new Vector3(45f, 3.2f, 6f);
    SpawnPos[1].localRotation *= Quaternion.Euler(0f,270f,0f);
    
    SpawnPos[2].position = new Vector3(10f, 3.2f, -19.5f);
    SpawnPos[2].localRotation *= Quaternion.Euler(0f,90,0f);

    SpawnPos[3].position = new Vector3(-10f, 3.2f, 5.5f);
    SpawnPos[3].localRotation *= Quaternion.Euler(0f, 90f, 0f);
    
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        if(this.gameObject.name == "Player")
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
            Die();
        }
        else if(health <= 0f && this.gameObject.name == "Player")
        {
            score2.Kills += 1;
            Die();
        }
    }

    void Die()
    {
        //Destroy(gameObject);
        int index = Random.Range(0,3);
        Debug.Log(index);



        Debug.Log(SpawnPos[index].position);

        health = 100f;
        if(this.gameObject.name == "Player")
        {

           distanceToSpawnPoint1 =  Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(27f, 3.2f, 34f));
           distanceToSpawnPoint2 =  Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(45f, 3.2f, 6f));
           distanceToSpawnPoint3 =  Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(10f, 3.2f, -19.5f));
           distanceToSpawnPoint4 =  Vector3.Distance(GameObject.Find("Player2").GetComponent<Transform>().position, new Vector3(-10f, 3.2f, 5.5f));

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
            healthUI.PlayerHealth = 100;
            Grenade1.enabled = true;
            Grenade2.enabled = true;

            weaponScript.Equip(1);            
            weaponScript.loadout[0] = null;

            weaponScript.grenadeAmount = 2;
            Debug.Log("Grenades: " + weaponScript.grenadeAmount);
        }


        else
        {
            
           distanceToSpawnPoint1 =  Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, new Vector3(27f, 3.2f, 34f));
           distanceToSpawnPoint2 =  Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, new Vector3(45f, 3.2f, 6f));
           distanceToSpawnPoint3 =  Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, new Vector3(10f, 3.2f, -19.5f));
           distanceToSpawnPoint4 =  Vector3.Distance(GameObject.Find("Player").GetComponent<Transform>().position, new Vector3(-10f, 3.2f, 5.5f));

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
            healthUI2.PlayerHealth = 100;            
            Player2Grenade1.enabled = true;
            Player2Grenade2.enabled = true;
        }
    }
}

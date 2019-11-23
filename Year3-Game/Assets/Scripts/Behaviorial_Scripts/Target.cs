using UnityEngine;
using System.Collections.Generic;
public class Target : MonoBehaviour
{

    public GameObject defaultSpawn;
    private List<Transform> SpawnPos = new List<Transform>();

    public float health = 50f;
    PlayerStatus healthUI;
    PlayerStatus2 healthUI2;
    void Start()
    {

        healthUI = GameObject.FindObjectOfType<PlayerStatus>();
        healthUI2 = GameObject.FindObjectOfType<PlayerStatus2>();

        //generate respawn transforms
         for(int i = 0; i < 4; i++)
         {
             Transform temp = Instantiate(defaultSpawn.transform); 
             SpawnPos.Add(temp);
             //temp.transform.parent = this.transform;
         }

         //set weapon spawn locations

    SpawnPos[0].position = new Vector3(27f, 3.65f, 34f);
    SpawnPos[0].localRotation *= Quaternion.Euler(0f,180f,0f);

    SpawnPos[1].position = new Vector3(45f, 3.65f, 6f);
    SpawnPos[1].localRotation *= Quaternion.Euler(0f,270f,0f);
    
    SpawnPos[2].position = new Vector3(10f, 3.65f, -19.5f);
    SpawnPos[2].localRotation *= Quaternion.Euler(0f,90,0f);

    SpawnPos[3].position = new Vector3(-10f, 3.65f, 5.5f);
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
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //Destroy(gameObject);
        int index = Random.Range(0,3);
        Debug.Log(index);

        this.gameObject.transform.position = SpawnPos[index].position;
        this.gameObject.transform.localRotation = SpawnPos[index].localRotation;

        Debug.Log(SpawnPos[index].position);

        health = 100f;
        if(this.gameObject.name == "Player")
        {
            healthUI.PlayerHealth = 100;
        }
        else
        {
            healthUI2.PlayerHealth = 100;
        }
    }
}

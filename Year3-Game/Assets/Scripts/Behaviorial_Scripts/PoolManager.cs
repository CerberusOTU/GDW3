using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject bulletHole;
    public int spawnCount;
    public Queue<GameObject> holeList = new Queue<GameObject>();

    private void Start()
    {
       makeList();
    }

    void makeList()
    {
         for(int i =0; i < spawnCount; i++)
        {
            GameObject temp = Instantiate(bulletHole);   
            holeList.Enqueue(temp);         

            temp.transform.parent = this.transform;
            temp.SetActive(false);
        }
    }

    public GameObject GetBulletHole()
    {
        GameObject objectToSpawn = holeList.Dequeue();
        objectToSpawn.SetActive(true);
        
        /*GameObject temp = Instantiate(bulletHole); 
        temp.SetActive(false);
        */
        holeList.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject bulletHole;
    public int spawnCount;
    
    public Queue<GameObject> bulletHoleList = new Queue<GameObject>();

    private void Start()
    {
       makeList();
    }

    void makeList()
    {
         for(int i =0; i < spawnCount; i++)
        {
            GameObject temp = Instantiate(bulletHole); 
            temp.SetActive(false);
            bulletHoleList.Enqueue(temp);
        }
    }

    public GameObject GetBulletHole()
    {
        GameObject objectToSpawn = bulletHoleList.Dequeue();
        objectToSpawn.SetActive(true);
        
        GameObject temp = Instantiate(bulletHole); 
        temp.SetActive(false);
        bulletHoleList.Enqueue(temp);

        return objectToSpawn;
    }
}

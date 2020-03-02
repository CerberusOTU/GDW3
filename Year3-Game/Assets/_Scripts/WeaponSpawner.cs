using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public Transform[] locations;
    public GameObject[] guns;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < locations.Length; i++)
        {
           GameObject GO;
           int id = Random.Range(0,4);
           Debug.Log(id);
           if(id == 0)
           {
               GO = Instantiate(guns[0],locations[i].position, locations[i].rotation);
               GO.name = "Tommy";
           }
           else if(id == 1)
           {
               GO = Instantiate(guns[1],locations[i].position, locations[i].rotation);
               GO.name = "Revolver";
           }
           else if(id == 2)
           {
               GO = Instantiate(guns[2],locations[i].position, locations[i].rotation);
               GO.name = "MP40";
           }
           else if(id == 3)
           {
               GO = Instantiate(guns[3],locations[i].position, locations[i].rotation);
               GO.name = "Shotgun";
           }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

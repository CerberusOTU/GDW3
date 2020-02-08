using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponRandomizer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Sphere;

    [Header("Gun Spawn Points")]
    [SerializeField] private Transform[] spawnlocations;
    public List<GameObject> sceneWeapons;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnlocations = GetComponentsInChildren<Transform>();
        spawnGuns();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnGuns()
    {
        for (int index = 0; index < spawnlocations.Length; index++)
        {
            int randGun = Random.Range(0, 100); //Random 0-100
            if (randGun >= 0 && randGun < 20) //20%
            {
                //Instantiate temp obj tommy
                GameObject temp = Instantiate(Sphere, spawnlocations[index].position, Quaternion.identity);
                sceneWeapons.Add(temp);
            }
            else if (randGun >= 20 && randGun < 50) //30%
            {
                //Instantiate temp obj mp40
                GameObject temp = Instantiate(Sphere, spawnlocations[index].position, Quaternion.identity);
                sceneWeapons.Add(temp);
            }
            else if (randGun >= 50 && randGun < 70) //20%
            {
                //Instantiate temp obj shotgun
                GameObject temp = Instantiate(Sphere, spawnlocations[index].position, Quaternion.identity);
                sceneWeapons.Add(temp);
            }
            else if (randGun >= 70 && randGun <= 100) //30%
            {
                //Instantiate temp obj revolver
                GameObject temp = Instantiate(Sphere, spawnlocations[index].position, Quaternion.identity);
                sceneWeapons.Add(temp);
            }
        }
    }

    public void despawnGuns()
    {
        for (int index = 0; index < sceneWeapons.Count; index++)
        {
            Destroy(sceneWeapons[index]);
        }
    }
}

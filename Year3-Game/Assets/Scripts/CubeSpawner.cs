using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectPooler.instance.spawnFromPool("CubeSpawner", transform.position,Quaternion.identity);
    }
}

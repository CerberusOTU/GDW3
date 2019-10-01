using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForce : MonoBehaviour, IPooledObject
{
    public float vertForce = 1f;
    public float horForce = .1f;

    // Start is called before the first frame update
    public void onObjectSpawn()
    {
        float xForce = Random.Range(-horForce,horForce);
        float yForce = Random.Range(vertForce / 2f, vertForce);
        float zForce = Random.Range(-horForce,horForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }
}

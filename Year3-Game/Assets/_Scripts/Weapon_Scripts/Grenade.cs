using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grenade : MonoBehaviour
{
    //public CameraShake cameraShake;
    public GameObject explosionEffect;
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    float countdown;
    bool hasExploded = false;


    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;

        }
    }

    void Explode()
    {
        GameObject tempEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObj in colliders)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Throwables Effects/Grenade Explode", GetComponent<Transform>().position);
         //   StartCoroutine(cameraShake.Shake(.15f,.4f));

            Rigidbody rb = nearbyObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force * 10f, transform.position, radius);
                if (SceneManager.GetActiveScene().name == "SampleScene")
                {
                    if (rb.gameObject.name == "Player")
                    {
                        GameObject.Find("Player").GetComponent<Target>().takeDamage(100f);
                    }
                    else if (rb.gameObject.name == "Player2")
                    {
                        GameObject.Find("Player2").GetComponent<Target>().takeDamage(100f);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}


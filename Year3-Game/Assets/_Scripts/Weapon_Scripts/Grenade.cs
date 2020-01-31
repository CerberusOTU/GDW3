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
    float dist;

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

    //void findDist()
    //{
    //    dist = Vector3.Distance
    //    (rb.position, this.transform.position);
    //    Debug.Log("DISTANCE   " + dist);
    //}
    void Explode()
    {
        GameObject tempEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObj in colliders)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Throwables Effects/Grenade Explode", GetComponent<Transform>().position);

            //StartCoroutine(cameraShake.Shake(.15f,.4f));

            Rigidbody rb = nearbyObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Explode");
                dist = Vector3.Distance
                (rb.position, this.transform.position);
                Debug.Log("DISTANCE   " + dist);
                rb.AddExplosionForce(force * 10f, transform.position, radius);
                if (SceneManager.GetActiveScene().name == "MainGame")
                {                        
                    Debug.Log("Explode2");
                    if (rb.gameObject.name == "Player")
                    {
                        if (dist <= 3)
                        GameObject.Find("Player").GetComponent<Target>().takeDamage(100f);
                        else if (dist > 3 && dist <= 3.5)
                            GameObject.Find("Player").GetComponent<Target>().takeDamage(80f);
                        else if (dist > 3.5 && dist <= 4)
                            GameObject.Find("Player").GetComponent<Target>().takeDamage(70f);
                        else if (dist > 4 && dist <= 4.5)
                            GameObject.Find("Player").GetComponent<Target>().takeDamage(50f);
                        else if (dist > 4.5 && dist <= 5)
                            GameObject.Find("Player").GetComponent<Target>().takeDamage(30f);


                    }
                    else if (rb.gameObject.name == "Player2")
                    {
                        if (dist <= 3)
                            GameObject.Find("Player2").GetComponent<Target>().takeDamage(100f);
                        else if (dist > 3 && dist <= 3.5)
                            GameObject.Find("Player2").GetComponent<Target>().takeDamage(80f);
                        else if (dist > 3.5 && dist <= 4)
                            GameObject.Find("Player2").GetComponent<Target>().takeDamage(70f);
                        else if (dist > 4 && dist <= 4.5)
                            GameObject.Find("Player2").GetComponent<Target>().takeDamage(50f);
                        else if (dist > 4.5 && dist <= 5)
                            GameObject.Find("Player2").GetComponent<Target>().takeDamage(30f);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}


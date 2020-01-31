using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehave : MonoBehaviour
{
    //public Camera player;
    //float dist;

    void OnEnable()
    {
        StartCoroutine(autostart());
        //Disable();
    }

    //void farDist()
    //{
    //    dist = Vector3.Distance
    //    (player.transform.position, this.transform.position);
    //    Debug.Log("DISTANCE   " + dist);
    //}
    IEnumerator autostart()
    {
        yield return new WaitForSeconds(10f);
        this.transform.gameObject.SetActive(false);
    }
    void OnBecameInvisible()
    {
        Disable();

    }
    //void Update()
    //{
    //     farDist();
    //}
    void Disable()
    {
        this.transform.gameObject.SetActive(false);

    }

}



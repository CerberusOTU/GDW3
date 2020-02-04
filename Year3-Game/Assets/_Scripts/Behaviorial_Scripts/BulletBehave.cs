using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehave : MonoBehaviour
{
    //public Camera player;
    //float dist;
    public ParticleSystem woodHit;
    public ParticleSystem stoneHit;
    public ParticleSystem metalHit;
    Weapon weapon;
    void Start()
    {
        weapon = GameObject.FindObjectOfType<Weapon>();
    }
    void OnEnable()
    {
        
        if (GameObject.Find("Player").GetComponent<Weapon>().hitInfo.collider.tag == "Metal")
            metalHit.Emit(1);
        else if (GameObject.Find("Player").GetComponent<Weapon>().hitInfo.collider.tag == "Wood")
            woodHit.Emit(1);
        else if (GameObject.Find("Player").GetComponent<Weapon>().hitInfo.collider.tag == "Concrete")
            stoneHit.Emit(1);
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



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
    playerWeaponManager weapon;
    void Start()
    {
        weapon = GameObject.FindObjectOfType<playerWeaponManager>();
    }
    void OnEnable()
    {
        
        if (GameObject.Find("Player").GetComponent<playerWeaponManager>().hitInfo.collider.tag == "Metal")
            metalHit.Emit(1);
        else if (GameObject.Find("Player").GetComponent<playerWeaponManager>().hitInfo.collider.tag == "Wood")
            woodHit.Emit(1);
        else if (GameObject.Find("Player").GetComponent<playerWeaponManager>().hitInfo.collider.tag == "Concrete")
            stoneHit.Emit(1);
        StartCoroutine(autostart());

    }

    IEnumerator autostart()
    {
        yield return new WaitForSeconds(5f);
        this.transform.gameObject.SetActive(false);
    }
   

    

}



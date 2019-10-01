using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Text scoreText;

    //Basic Gun Properties
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 10f;

    //Reload Properties
    public int maxAmmo = 30;
    private int currentAmmo = -1;
    public float reloadTime = 1f;
    private bool isReloading = false;

    //Recoil Properties
    public float maxRecoil_x = -20f;
    public float recoilSpeed = 10f;
    private float currentRecoil = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    private float nextTimetoFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine whether reloading or out of ammo
        if (isReloading)
            return;

        if (currentAmmo <=0)
        {
            scoreText.text = "Reloading";
            StartCoroutine(Reload());
            return;
        }


        else scoreText.text = currentAmmo.ToString();

        //player has inputted time to shoot
        if (Input.GetButton("Fire1") && Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + 1f / fireRate;
            
            //Recoil
            currentRecoil += 0.1f;

            Shoot();
        }
        

        //As long as clip is not full, player can reload
        if (Input.GetKey(KeyCode.R) && currentAmmo != maxAmmo)
        {
            scoreText.text = "Reloading";
            StartCoroutine(Reload());
        }

        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }


    void Shoot()
    {
        currentAmmo--;
        muzzleFlash.Play();
        RaycastHit hitInfo;

        //Add recoil to move the camera and player orientation
        if (currentRecoil > 0)
        {
            Quaternion maxRecoil = Quaternion.Euler (maxRecoil_x, 0f, 0f);
            fpsCam.transform.localRotation = Quaternion.Slerp(fpsCam.transform.localRotation,maxRecoil,Time.deltaTime * recoilSpeed);
            currentRecoil -= Time.deltaTime;
        }
        else
        {
            currentRecoil = 0f;
            fpsCam.transform.localRotation = Quaternion.Slerp (fpsCam.transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }

        //check if we actually hit anything
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);

            //find the target component on obect we just hit and store it
            Target target = hitInfo.transform.GetComponent<Target>();

            //apply damage to target
            if (target != null)
            {
                target.takeDamage(damage);
            }

            //apply force to target on impact
            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }

            
        }
    }
}

using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 10f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    private float nextTimetoFire = 0f;

    // Update is called once per frame
    void Update()
    {
        //player has inputted time to shoot
        if (Input.GetButton("Fire1") && Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hitInfo;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public _Gun[] loadout;
    public Transform weaponParent;

    private int currentIndex;

    private GameObject currentWeapon;
    
    public Canvas crossHair;
    public Canvas hitMark;
    
    public Camera cam;
    //BulletHole Variables ////
     private PoolManager _pool;
    //////////////////////////

    private float currentRecoil;
    private float tempTime;
    private float timeFiringHeld;

    private float adjustedBloom;

    private float currentCool;

   private Motion player;

   //Ammo UI///
    public Text AmmoText;
   ///////////////////
    
    void Start()
    {
         _pool = GameObject.FindObjectOfType<PoolManager>();
         player = GameObject.FindObjectOfType<Motion>();
         hitMark.enabled = false;

         loadout[0].currentAmmo = loadout[0].maxAmmo;
         loadout[1].currentAmmo = loadout[1].maxAmmo;
    }

    void Update()
    {
        AmmoText.text = loadout[currentIndex].currentAmmo.ToString() + "/" +  loadout[currentIndex].maxAmmo.ToString();

        if (loadout[currentIndex].isReloading)
        {
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime *4f); 
            return;
        }
        if (loadout[currentIndex].currentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetKey(KeyCode.R) && loadout[currentIndex].currentAmmo != loadout[currentIndex].maxAmmo)
        {
            StartCoroutine(Reload());
        }
        
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            Equip(0);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            Equip(1);
        }
        
        if(currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if(Input.GetMouseButton(0) && currentCool <= 0)
            {
                currentRecoil += 0.1f;
                Shoot();
            }
            
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime *4f); 
        }

        

        if(currentCool > 0)
        {
            currentCool -= Time.deltaTime;
        }
    }

    void Equip(int _ind)
    {
        if(currentWeapon != null) 
        {
            Destroy(currentWeapon);
        }

        currentIndex = _ind;

        GameObject newWeapon = Instantiate(loadout[_ind].obj, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject; 
        newWeapon.transform.localPosition = Vector3.zero;   
        newWeapon.transform.localEulerAngles = Vector3.zero;   

        currentWeapon = newWeapon;
    }

    void Aim(bool isAiming)
    {
        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform ADS = currentWeapon.transform.Find("States/ADS");
        Transform Hip = currentWeapon.transform.Find("States/Hip");

        if(isAiming && !player.isSprinting)
        {
            //ADS
            anchor.position = Vector3.Lerp(anchor.position, ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            crossHair.enabled = false;
        }
        else
        {
            //Hip
            anchor.position = Vector3.Lerp(anchor.position, Hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            crossHair.enabled = true;
        }

    }

    void Shoot()
    {
        Transform spawn = cam.transform;
        loadout[currentIndex].currentAmmo--;

        if(Input.GetMouseButton(1))
        {
             adjustedBloom = loadout[currentIndex].bloom / 3;
        }
        else
        {
             adjustedBloom = loadout[currentIndex].bloom;
        }
        //bloom
        Vector3 bloom = spawn.position + spawn.forward * 1000f;
        bloom += Random.Range(-adjustedBloom, adjustedBloom) * spawn.up;
        bloom += Random.Range(-adjustedBloom, adjustedBloom) * spawn.right;
        bloom -= spawn.position;
        bloom.Normalize();

        ///-----Recoil-----/////
        if (Input.GetMouseButtonDown(0))
        {
            tempTime = Time.time;
        }
         if (currentRecoil > 0)
        {
            timeFiringHeld = Time.time - tempTime;
            Quaternion maxRecoil = Quaternion.Euler(cam.transform.localEulerAngles.x + loadout[currentIndex].maxRecoil_x, 0f, 0f);
            Debug.Log(maxRecoil);
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation,maxRecoil,Time.deltaTime * loadout[currentIndex].recoilSpeed * Mathf.Lerp(1,loadout[currentIndex].recoilDampen,timeFiringHeld));
            currentRecoil -= Time.deltaTime;
        }
        else
        {
            currentRecoil = 0f;
            cam.transform.localRotation = Quaternion.Slerp (cam.transform.localRotation, Quaternion.identity, Time.deltaTime * loadout[currentIndex].recoilSpeed / 2);
        }


        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(spawn.position, bloom, out hitInfo, 100f))
        {
            Debug.Log(hitInfo.transform.name);

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                 StartCoroutine(displayHitmark());
                target.takeDamage(10f);
            }

            //check if we hit a wall so we can display bulletholes
            if(hitInfo.collider.tag == "Wall")
            {
                
                for(int i = 0; i < _pool.holeList.Count; i++)
                {
                    //if object is inactive in list, use it
                    if(_pool.holeList[i].activeInHierarchy == false)
                    {
                        _pool.holeList[i].SetActive(true);
                        _pool.holeList[i].transform.position = hitInfo.point;
                        _pool.holeList[i].transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                        break;
                    }
                   /*  //in case we go through the entire list and require more bullet holes, create some  
                    else
                    {
                        if(i == _pool.holeList.Count - 1)
                        {
                        GameObject newBullet = Instantiate(_pool.bulletHole) as GameObject;
                        newBullet.transform.parent = _pool.transform;
                        newBullet.SetActive(false);
                        _pool.holeList.Add(newBullet);               
                        }     
                    } */
                }
            }
             
        }

        //GUN FX
       // currentWeapon.transform.Rotate(loadout[currentIndex].recoil, 0, 0);
        currentWeapon.transform.position -= -currentWeapon.transform.forward * loadout[currentIndex].kickBack;
        currentCool = loadout[currentIndex].firerate;
    }   

     IEnumerator displayHitmark()
    {
        hitMark.enabled = true;

        yield return new WaitForSeconds(0.05f);
        hitMark.enabled = false;
    }

     IEnumerator Reload()
    {
        loadout[currentIndex].isReloading = true;

        yield return new WaitForSeconds(loadout[currentIndex].reloadTime);

        loadout[currentIndex].currentAmmo = loadout[currentIndex].maxAmmo;
        loadout[currentIndex].isReloading = false;
        tempTime = Time.time;
    }
}

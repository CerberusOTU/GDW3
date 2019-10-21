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
    private bool origPosReset = true;
    Quaternion saveInitShot;
    private float timeFiringHeld;

    private float adjustedBloom;

    private float currentCool;

   private Motion player;
   //Ammo UI///
    public Text AmmoText;
    public Text AmmoText2;
    public Image WeaponSlot1;
    public Image WeaponSlot2;
    public Image MainWeapon;
    public Image SideWeapon;

   Vector3 temp;
    Vector3 temp2;
 

   ///////////////////

    void Start()
    {
         _pool = GameObject.FindObjectOfType<PoolManager>();
         player = GameObject.FindObjectOfType<Motion>();
         hitMark.enabled = false;

        temp2 = transform.localScale;
        temp2.x = 1f;
        temp2.y = 1f;


         loadout[0].currentAmmo = loadout[0].maxAmmo;
         loadout[1].currentAmmo = loadout[1].maxAmmo;

         Equip(0);
         
    }

    void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");

        AmmoText.text = loadout[0].currentAmmo.ToString();
        AmmoText2.text = loadout[1].currentAmmo.ToString();

        if (loadout[currentIndex] == loadout[0])
        {
            temp = transform.localScale;

            temp.x = 0.75f;
            temp.y = 0.75f;


         //  AmmoText2.transform.localScale = temp;
            WeaponSlot1.transform.localScale = temp;
         //   SideWeapon.transform.localScale = temp;
            //////////
          //  AmmoText.transform.localScale = temp2;
            WeaponSlot2.transform.localScale = temp2;
          //  MainWeapon.transform.localScale = temp2;
        }
        if (loadout[currentIndex] == loadout[1])
        {
            temp = transform.localScale;

            temp.x = 0.75f;
            temp.y = 0.75f;


           // AmmoText.transform.localScale = temp;
            WeaponSlot2.transform.localScale = temp;
           // MainWeapon.transform.localScale = temp;
            //////
           // AmmoText2.transform.localScale = temp2;
            WeaponSlot1.transform.localScale = temp2;
            //SideWeapon.transform.localScale = temp2;

        }

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
        
        //d > 0f is scrolling up
        if(Input.GetKeyUp(KeyCode.Alpha1) && currentIndex != 0 || d > 0f && currentIndex != 0)
        {
            Equip(0);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2) && currentIndex != 1 || d < 0f && currentIndex != 1)
        {
            Equip(1);
        }
        
        if(currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if(Input.GetMouseButton(0) && currentCool <= 0)
            {
                origPosReset = false;
                Shoot();
            }
            // Return back to original left click position
            if (!Input.GetMouseButton(0) && origPosReset == false)
            {
                cam.transform.localRotation = Quaternion.Slerp (cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
                if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    Debug.Log(origPosReset);
                    origPosReset = true;
                }
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
            saveInitShot = Quaternion.Euler(cam.transform.localEulerAngles.x,0f,0f);

        }

        // Recoil Dampen
        timeFiringHeld = Time.time - tempTime;
        Quaternion maxRecoil = Quaternion.Euler(cam.transform.localEulerAngles.x + loadout[currentIndex].maxRecoil_x, 0f, 0f);
        cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, maxRecoil, Time.deltaTime * loadout[currentIndex].recoilSpeed * Mathf.Lerp(1,loadout[currentIndex].recoilDampen,timeFiringHeld));


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public _Gun[] loadout;
    public Transform weaponParent;

    private int currentIndex;

    private GameObject currentWeapon;
    
    public Canvas crossHair;
    
    public Camera cam;
    //BulletHole Variables ////
     private PoolManager _pool;
    //////////////////////////
    
    void Start()
    {
         _pool = GameObject.FindObjectOfType<PoolManager>();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            Equip(0);
        }
        
        if(currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if(Input.GetMouseButton(0))
            {
                Shoot();
            }
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

        if(isAiming)
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
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 100f))
        {
            Debug.Log(hitInfo.transform.name);

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
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
                    //in case we go through the entire list and require more bullet holes, create some  
                    else
                    {
                        if(i == _pool.holeList.Count - 1)
                        {
                        GameObject newBullet = Instantiate(_pool.bulletHole) as GameObject;
                        newBullet.transform.parent = _pool.transform;
                        newBullet.SetActive(false);
                        _pool.holeList.Add(newBullet);               
                        }     
                    }
                }
            }
             
        }

    }   
}

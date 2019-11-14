using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;

public class Weapon : MonoBehaviour
{

    //Controller Variables//
    Controller controller;

    float vibrateVal = 0;

    //////////////////////
   public _Gun[] loadout;
    
    public GameObject[] gunMeshes;
    //0 = Tommy
    //1 = Revolver
    //2 = MP40
    //3 = Shotgun

    public GameObject[] inSceneGuns;

    public _Gun[] scriptOBJ;
    //0 = Tommy
    //1 = Revolver
    //2 = MP40
    //3 = Shotgun

    public GameObject defaultSpawn;
    private List<Transform> weaponSpawnPos = new List<Transform>();
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
    public Text Reloading;
    public Text PickUp;
    public Image WeaponSlot1;
    public Image WeaponSlot2;
    public Image MainWeapon;
    public Image SideWeapon;

   Vector3 temp;
    Vector3 temp2;

    public ParticleSystem muzzleFlash;
    Transform tempMuzzle;
   ///////////////////
    Rigidbody rigid;

    //GUNS//
    public _Gun M1911;
    public _Gun Tommy;
    public _Gun MP40;
    public _Gun Shotgun;

    /////////////////////////

    //to switch our gun meshes in scene
    bool isSwitched = false;
    RaycastHit checkWeapon;

    //Throwing Grenade
    
    //Throwing Grenade
    private bool isCookingNade = false;
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody>();
        
         _pool = GameObject.FindObjectOfType<PoolManager>();
         player = GameObject.FindObjectOfType<Motion>();
         hitMark.enabled = false;
         controller = GameObject.FindObjectOfType<Controller>();
         
        temp2 = transform.localScale;
        temp2.x = 1f;
        temp2.y = 1f;

        //M1911 reset ammo
        loadout[1].currentAmmo = loadout[1].clipSize;
        loadout[1].maxAmmo = loadout[1].alwaysMax;
        loadout[1].isReloading = false;
        //primary guns reset ammo
        for(int i = 0; i < scriptOBJ.Length; i++)
        {
            scriptOBJ[i].currentAmmo = scriptOBJ[i].clipSize;
            scriptOBJ[i].maxAmmo = scriptOBJ[i].alwaysMax;
            scriptOBJ[i].isReloading = false;
        }

         Equip(0);

        //generate spawn transforms
         for(int i = 0; i < 4; i++)
         {
             Transform temp = Instantiate(defaultSpawn.transform); 
             weaponSpawnPos.Add(temp);
             temp.transform.parent = this.transform;
         }

    //set weapon spawn locations

    weaponSpawnPos[0].position = new Vector3(16.94f, 3.728f, 9.426f);
    weaponSpawnPos[0].localRotation *= Quaternion.Euler(0f,90f,90f);

    weaponSpawnPos[1].position = new Vector3(11.73f, 4.792f, -6.374f);
    weaponSpawnPos[1].localRotation *= Quaternion.Euler(0f,90f,0f);
    
    weaponSpawnPos[2].position = new Vector3(8.876f, 3.68f, 6.75f);
    weaponSpawnPos[2].localRotation *= Quaternion.Euler(0f,160f,90f);

    weaponSpawnPos[3].position = new Vector3(0.14f, 4.26f, 3.161f);
    weaponSpawnPos[3].localRotation *= Quaternion.Euler(0f, 0f, 0f);

        //random index number for spawn locations
        var numList = new List<int>();
        for(int k = 0; k < weaponSpawnPos.Count; k++)
        {
            numList.Add(k);
        }
        numList = numList.OrderBy(i => Random.value).ToList();

        //set the in scene guns to the random transforms
        for(int i = 0; i < inSceneGuns.Length; i++)
        { 
            inSceneGuns[i].transform.position = weaponSpawnPos[numList[i]].position;
            inSceneGuns[i].transform.localRotation = weaponSpawnPos[numList[i]].localRotation;
        }

    }

    void FixedUpdate()
    {
       // GamePad.SetVibration(controller.playerIndex, controller.state.Triggers.Right, controller.state.Triggers.Right);
    }

    void Update()
    {
        
        controller.prevState = controller.state;
        controller.state = GamePad.GetState(controller.playerIndex);

        if(controller.state.Triggers.Right == 1)
        {
            vibrateVal = 0.5f;
        }
        else
        {
            vibrateVal = 0;
        }
        ///////////////////////////////////
        
        PickUp.enabled = false;
        SwitchWeapon();

        float d = Input.GetAxis("Mouse ScrollWheel");

        AmmoText.text = loadout[0].currentAmmo.ToString() + " / " + loadout[0].maxAmmo.ToString();
        AmmoText2.text = loadout[1].currentAmmo.ToString() + " / " + loadout[1].maxAmmo.ToString();;

        if (Input.GetKeyDown(KeyCode.Space) || controller.prevState.Buttons.A == ButtonState.Released && controller.state.Buttons.A == ButtonState.Pressed)
        {
            Debug.Log("I Jumped");

            if (rigid != null)
            {
                rigid.AddForce(transform.up * 2f, ForceMode.Impulse);
            }
                
        }

        if(Input.GetKey(KeyCode.G) || controller.state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            isCookingNade = true;
            throwGrenade();
        }else if (Input.GetKeyUp(KeyCode.G) || controller.prevState.Buttons.RightShoulder == ButtonState.Pressed && controller.state.Buttons.RightShoulder == ButtonState.Released)
        {
            isCookingNade = false;
            throwGrenade();
        }

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
            Reloading.text = "Reloading...";
            return;
        }
        if (loadout[currentIndex].currentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if ((Input.GetKey(KeyCode.R) || controller.prevState.Buttons.X == ButtonState.Released && controller.state.Buttons.X == ButtonState.Pressed) && loadout[currentIndex].currentAmmo != loadout[currentIndex].clipSize)
        {
            StartCoroutine(Reload());
        }
        
        //d > 0f is scrolling up
        if((Input.GetKeyUp(KeyCode.Alpha1) || controller.prevState.Buttons.Y == ButtonState.Released && controller.state.Buttons.Y == ButtonState.Pressed) && currentIndex != 0 || d > 0f && currentIndex != 0)
        {
            Equip(0);
        }
        else if((Input.GetKeyUp(KeyCode.Alpha2) || controller.prevState.Buttons.Y == ButtonState.Released && controller.state.Buttons.Y == ButtonState.Pressed)  && currentIndex != 1 || d < 0f && currentIndex != 1)
        {
            Equip(1);
        }
        
        if(currentWeapon != null)
        {
            Aim((Input.GetMouseButton(1) || controller.state.Triggers.Left == 1));

            if(Input.GetMouseButtonDown(0) && currentCool <= 0 && loadout[currentIndex].ShotType == "Single" && loadout[currentIndex].maxAmmo >= 0)
            {
                origPosReset = false;
                Shoot();
            }
            else if((Input.GetMouseButton(0) || controller.state.Triggers.Right == 1) && currentCool <= 0 && loadout[currentIndex].ShotType == "Auto" && loadout[currentIndex].maxAmmo >= 0)
            {
                origPosReset = false;
                Shoot();
            }
            // Return back to original left click position
            if ((!Input.GetMouseButton(0) || controller.state.Triggers.Right == 0)  && origPosReset == false)
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

        if (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
        {
            tempMuzzle = currentWeapon.transform.Find("States/ADS/MuzzlePos");
            muzzleFlash.transform.position = tempMuzzle.position;
        }
        else
        {
            tempMuzzle = currentWeapon.transform.Find("States/Hip/MuzzlePos");
            muzzleFlash.transform.position = tempMuzzle.position;
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
        muzzleFlash.Play();

        Transform spawn = cam.transform;
        loadout[currentIndex].currentAmmo--;
        
        if(Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
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

        Vector3[] bloomShotty = new Vector3[8];
        for(int i =0; i < 8; i++)
        {
            bloomShotty[i] = spawn.position + spawn.forward * 1000f;
            bloomShotty[i] += Random.Range(-adjustedBloom, adjustedBloom) * spawn.up;
            bloomShotty[i] += Random.Range(-adjustedBloom, adjustedBloom) * spawn.right;
            bloomShotty[i] -= spawn.position;
            bloomShotty[i].Normalize();
        }

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

        
        //bloom
        if(loadout[currentIndex].className == "Shotgun")
        {
            Target target;
            for(int j =0; j < 8;j++)
            {

            Physics.Raycast(spawn.position, bloomShotty[j], out hitInfo, 100f);
            
            target = hitInfo.transform.GetComponent<Target>();

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
                        _pool.holeList[i].transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
                        _pool.holeList[i].transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                        break;
                    }
                }
            }
        
            }    
        }
        else if(loadout[currentIndex].className != "Shotgun")
        {
            Physics.Raycast(spawn.position, bloom, out hitInfo, 100f);
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
                        _pool.holeList[i].transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
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
        
        loadout[currentIndex].maxAmmo = loadout[currentIndex].maxAmmo - (loadout[currentIndex].clipSize - loadout[currentIndex].currentAmmo);
        loadout[currentIndex].currentAmmo = loadout[currentIndex].clipSize;
        loadout[currentIndex].isReloading = false;
        Reloading.text = " ";

        tempTime = Time.time;
    }

     void SwitchWeapon()
    {
         checkWeapon = new RaycastHit();

        //if we hit something
         if(Physics.Raycast(cam.transform.position, cam.transform.forward, out checkWeapon, 3f))
        {
            //if it is tagged as a weapon
            if (checkWeapon.collider.tag == "Weapon")
            {
                PickUp.enabled = true;
                PickUp.text = "Press E to pick up " + checkWeapon.collider.name;
                
                //if the user presses E
                if (Input.GetKeyDown(KeyCode.E) && currentIndex == 0)
                {
                    if(checkWeapon.collider.name == "Revolver")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();
                        
                        GameObject tempMesh = null;

                        if(loadout[0].name == "Tommy")
                        {
                            Debug.Log(loadout[0].name);
                            tempMesh = gunMeshes[0];
                        }
                        else if(loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        } 
                        else if(loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else if (loadout[0].name == "Shotgun")
                        {
                            tempMesh = gunMeshes[3];
                        }

                        GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                        switched.name = loadout[0].name;
                        Destroy(checkWeapon.collider.gameObject);

                        
                        loadout[0] = scriptOBJ[1];
                        Equip(0);
    

                    }
                    else if(checkWeapon.collider.name == "Tommy")
                    {

                        Transform temp = checkWeapon.collider.GetComponent<Transform>();
                        
                        GameObject tempMesh = null;
                        if(loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else  if(loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        }
                        else if(loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                        }
                        else if (loadout[0].name == "Shotgun")
                        {
                            tempMesh = gunMeshes[3];
                        }

                        GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                        switched.name = loadout[0].name;
                        Destroy(checkWeapon.collider.gameObject);

                        
                        loadout[0] = scriptOBJ[0];
                        Equip(0);
                    }
                    else if (checkWeapon.collider.name == "MP40")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();
                        
                        GameObject tempMesh = null;
                        if(loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                        }
                        else if(loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else if(loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        }
                        else if (loadout[0].name == "Shotgun")
                        {
                            tempMesh = gunMeshes[3];
                        }
                        GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                        switched.name = loadout[0].name;
                        Destroy(checkWeapon.collider.gameObject);
                        
                        loadout[0] = scriptOBJ[2];
                        Equip(0);

                    }
                    else if (checkWeapon.collider.name == "Shotgun")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;

                        
                        if (loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        }
                        else if (loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else if (loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                        }
                       else if (loadout[0].name == "Shotgun")
                        {
                            tempMesh = gunMeshes[3];
                        }

                        GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                        switched.name = loadout[0].name;
                        Destroy(checkWeapon.collider.gameObject);


                        loadout[0] = scriptOBJ[3];
                        Equip(0);


                    }
                }
            }
        }
    }


    Rigidbody rb_Grenade;
    bool isNewNade = true;
    void throwGrenade()
    {
       if (isNewNade)
        {
            GameObject grenade = Instantiate(grenadePrefab, cam.transform.position + (cam.transform.forward * 0.5f), cam.transform.rotation);
            rb_Grenade = grenade.GetComponent<Rigidbody>();
            isNewNade = false;
        }
        else
        {
            if (isCookingNade)
            {
            
                rb_Grenade.position = cam.transform.position + (cam.transform.forward * 0.5f);
                rb_Grenade.useGravity = false;
                rb_Grenade.freezeRotation = true;        
            }
            else if (!isCookingNade)
            {
                isNewNade = true;
                rb_Grenade.useGravity = true;
                rb_Grenade.freezeRotation = false;
                rb_Grenade.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);      
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
public class Weapon : MonoBehaviour
{
    public CameraShake cameraShake;
    //Controller Variables//
    Controller controller;

    bool m_isAxisInUseDown = false;
    bool m_isAxisInUseUp = false;
    bool buttonInUse = false;
    bool vibrate = false;
    bool shootDown = false;
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
    private float baseFOV;
    private float FOVmod = 0.90f;
    //BulletHole Variables ////
    private PoolManager _pool;
    //////////////////////////

    private float currentRecoil;
    private float tempTime;
    private bool origPosReset = true;
    Quaternion saveInitShot;
    private float timeFiringHeld;
    private bool reloadCancel = false;
    private float reloadDelay = 0.0f;

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

    public Image Grenade1;
    public Image Grenade2;

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

    public int grenadeAmount = 2;

    //Mesh Model Switch
    public GameObject RobertoTommy;
    public GameObject RobertoM1911;
    public GameObject RobertoRevolver;
    public GameObject RobertoMP40;
    public GameObject RobertoShotgun;


    public Image up_crosshair;
    public Image down_crosshair;
    public Image left_crosshair;
    public Image right_crosshair;

    //**************TUTORIAL VARIABLES**************/
    [System.NonSerialized]
    public Tutorial_Manager _tutManager;
    //Metrics Manager
    public MetricsLogger _metricsLogger;

    void Start()
    {
          baseFOV = cam.fieldOfView;
        rigid = this.gameObject.GetComponent<Rigidbody>();
        _pool = GameObject.FindObjectOfType<PoolManager>();
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager>();
        _metricsLogger = GameObject.FindObjectOfType<MetricsLogger>();
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
        for (int i = 0; i < scriptOBJ.Length; i++)
        {
            scriptOBJ[i].currentAmmo = scriptOBJ[i].clipSize;
            scriptOBJ[i].maxAmmo = scriptOBJ[i].alwaysMax;
            scriptOBJ[i].isReloading = false;
        }
        Equip(0);

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            //generate spawn transforms
            for (int i = 0; i < 4; i++)
            {
                Transform temp = Instantiate(defaultSpawn.transform);
                weaponSpawnPos.Add(temp);
                //temp.transform.parent = this.transform;
            }

            //set weapon spawn locations

            weaponSpawnPos[0].position = new Vector3(15.02f, 5f, -16.07f);
            weaponSpawnPos[0].localRotation *= Quaternion.Euler(0f, 90f, 0f);

            weaponSpawnPos[1].position = new Vector3(-15.779f, 4.92f, 10.895f);
            weaponSpawnPos[1].localRotation *= Quaternion.Euler(0f, 90f, 0f);

            weaponSpawnPos[2].position = new Vector3(15.123f, 4.75f, 9.866f);
            weaponSpawnPos[2].localRotation *= Quaternion.Euler(0f, -75f, 90f);

            weaponSpawnPos[3].position = new Vector3(24.451f, 5f, 29.07f);
            weaponSpawnPos[3].localRotation *= Quaternion.Euler(0f, -90f, 0f);

            //random index number for spawn locations
            var numList = new List<int>();
            for (int k = 0; k < weaponSpawnPos.Count; k++)
            {
                numList.Add(k);
            }

            numList = numList.OrderBy(i => Random.value).ToList();

            //set the in scene guns to the random transforms
            for (int i = 0; i < inSceneGuns.Length; i++)
            {
                inSceneGuns[i].transform.position = weaponSpawnPos[numList[i]].position;
                inSceneGuns[i].transform.localRotation = weaponSpawnPos[numList[i]].localRotation;
            }
        }
    }

    void FixedUpdate()
    {
        if ((controller.state.Triggers.Right == 1) && !PlayerisReloading && loadout[currentIndex].ShotType == "Auto" && loadout[currentIndex].maxAmmo >= 0)
        {
            GamePad.SetVibration((PlayerIndex)0, 0.5f, 0);
        }
        else
        {
            GamePad.SetVibration((PlayerIndex)0, 0, 0);
        }
    }

    void Update()
    {
        //for when game ends
        if (Time.timeScale != 1f)
        {
            crossHair.enabled = false;
        }
        ///////////////////////////////////

        PickUp.enabled = false;
        SwitchWeapon();
        Reload();
        MeshSwitch();

        if(player.isSprinting)
        {
            up_crosshair.transform.localPosition = new Vector3(up_crosshair.transform.localPosition.x, 20f, up_crosshair.transform.localPosition.z);
            down_crosshair.transform.localPosition = new Vector3(down_crosshair.transform.localPosition.x, -31.1f, down_crosshair.transform.localPosition.z);
            left_crosshair.transform.localPosition = new Vector3(-27.3f, left_crosshair.transform.localPosition.y, left_crosshair.transform.localPosition.z);
            right_crosshair.transform.localPosition = new Vector3(26.2f, right_crosshair.transform.localPosition.y, right_crosshair.transform.localPosition.z); 
        }

        float d = Input.GetAxis("Mouse ScrollWheel");

        if (loadout[0].maxAmmo >= 0 && loadout[0].currentAmmo >= 0)
        {
            AmmoText.text = loadout[0].currentAmmo.ToString() + " / " + loadout[0].maxAmmo.ToString();
        }
        else
        {
            AmmoText.text = "0 / 0";
        }

        if (loadout[1].maxAmmo >= 0 && loadout[1].currentAmmo >= 0)
        {
            AmmoText2.text = loadout[1].currentAmmo.ToString() + " / " + loadout[1].maxAmmo.ToString();
        }
        else
        {
            AmmoText2.text = "0 / 0";
        }

        if (grenadeAmount > 0)
            {
                if (Input.GetKey(KeyCode.G))
                {
                    isCookingNade = true;
                    throwGrenade();
                     if (!_tutManager.b_grenadeComplete)
                        _tutManager.Notify("GRENADE_COMPLETE");
                }
                //else if (Input.GetButtonUp("Grenade"))
                else if (Input.GetKeyUp(KeyCode.G))
                {
                    if (grenadeAmount == 2)
                    {
                        Grenade1.enabled = false;
                        isCookingNade = false;
                        throwGrenade();
                        grenadeAmount--;
                    }
                    else if (grenadeAmount == 1)
                    {
                        Grenade2.enabled = false;
                        isCookingNade = false;
                        throwGrenade();
                        grenadeAmount--;
                    }
                }
            }
        //if (Input.GetButton("Grenade"))
        if (controller.state.IsConnected)
        {
            if (grenadeAmount > 0)
            {
                if ((controller.state.Buttons.RightShoulder == ButtonState.Pressed) || Input.GetKey(KeyCode.G))
                {
                    isCookingNade = true;
                    throwGrenade();
                     if (!_tutManager.b_grenadeComplete)
                        _tutManager.Notify("GRENADE_COMPLETE");
                }
                //else if (Input.GetButtonUp("Grenade"))
                else if ((controller.state.Buttons.RightShoulder == ButtonState.Released && controller.prevState.Buttons.RightShoulder == ButtonState.Pressed) || Input.GetKeyUp(KeyCode.G))
                {
                    if (grenadeAmount == 2)
                    {
                        Grenade1.enabled = false;
                        isCookingNade = false;
                        throwGrenade();
                        grenadeAmount--;
                    }
                    else if (grenadeAmount == 1)
                    {
                        Grenade2.enabled = false;
                        isCookingNade = false;
                        throwGrenade();
                        grenadeAmount--;
                    }
                }
            }
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


        if (loadout[currentIndex].currentAmmo == 0 && !PlayerisReloading)
        {
            if (!PlayerisReloading)
            {
                reloadDelay = 0.0f;
            }
            reloadCancel = false;
            PlayerisReloading = true;

            if (loadout[currentIndex].maxAmmo > 0)
            {
                PlaySound(loadout[currentIndex].ReloadPath);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || controller.state.Buttons.X == ButtonState.Pressed && controller.prevState.Buttons.X == ButtonState.Released && loadout[currentIndex].currentAmmo != loadout[currentIndex].clipSize && !PlayerisReloading)
        {
            reloadCancel = false;
            PlayerisReloading = true;
            reloadDelay = 0.0f;
            PlaySound(loadout[currentIndex].ReloadPath);

        }

        //d > 0f is scrolling up
        if ((controller.state.Buttons.Y == ButtonState.Pressed && controller.prevState.Buttons.Y == ButtonState.Released && currentIndex != 0) || (d > 0f && currentIndex != 0))
        {
            reloadCancel = true;
            Equip(0);
        }
        else if ((controller.state.Buttons.Y == ButtonState.Pressed && controller.prevState.Buttons.Y == ButtonState.Released && currentIndex != 1) || (d < 0f && currentIndex != 1))
        {
            reloadCancel = true;
            Equip(1);
        }

        if (currentWeapon != null)
        {
            Aim((Input.GetMouseButton(1) || controller.state.Triggers.Left == 1));

            getShootDown();
            getShootUp();

            /* if((Input.GetMouseButtonDown(0) || shootDown == true) && currentCool <= 0 && loadout[currentIndex].ShotType == "Single" && loadout[currentIndex].maxAmmo >= 0)
            {
                origPosReset = false;
                Shoot();
            }
            else  */
            if (!PlayerisReloading && (Input.GetMouseButton(0) || controller.state.Triggers.Right == 1) && currentCool <= 0 && loadout[currentIndex].ShotType == "Auto" && loadout[currentIndex].currentAmmo > 0)
            {
                origPosReset = false;
                StartCoroutine(cameraShake.Shake(.05f,.0001f));
                Shoot();
            }
            // Return back to original left click position
            if ((!Input.GetMouseButton(0) || controller.state.Triggers.Right == 0) && origPosReset == false)
            {
                //cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
                //if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || controller.state.ThumbSticks.Right.Y != 0 || controller.state.ThumbSticks.Right.X != 0)
                //{
                //Debug.Log(origPosReset);
                //origPosReset = true;
                //}
            }

            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            up_crosshair.transform.localPosition = Vector3.Lerp(up_crosshair.transform.localPosition, new Vector3(0.6f, 4f, 0f), Time.deltaTime * 4f);
            down_crosshair.transform.localPosition = Vector3.Lerp(down_crosshair.transform.localPosition, new Vector3(0.6f, -15f, 0.0f), Time.deltaTime * 4f);
            left_crosshair.transform.localPosition = Vector3.Lerp(left_crosshair.transform.localPosition, new Vector3(-11.3f, -4f, 0.0f), Time.deltaTime * 4f);
            right_crosshair.transform.localPosition = Vector3.Lerp(right_crosshair.transform.localPosition, new Vector3(10.2f, -4f, 0.0f), Time.deltaTime * 4f);
        
        }



        if (currentCool > 0)
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
        if (currentWeapon != null)
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
        //Transform anchorR = currentWeapon.transform.Find("Anchor");

        Transform ADS = currentWeapon.transform.Find("States/ADS");
        Transform Hip = currentWeapon.transform.Find("States/Hip");
        //Transform Sprint = currentWeapon.transform.Find("States/Sprint");
        //Transform SprintR = currentWeapon.transform.Find("States/Sprint");


        if (isAiming && !player.isSprinting)
        {
            //ADS
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
            anchor.position = Vector3.Lerp(anchor.position, ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            crossHair.enabled = false;
        }
        //else if (!isAiming && player.isSprinting)
        //{ 
        //Quaternion tempRot = Quaternion.Euler(0f,-10f,0f);
        //Hip
        //anchor.position = Vector3.Lerp(anchor.position, Sprint.position, Time.deltaTime * loadout[currentIndex].aimSpeed);

        //anchor.localRotation = Quaternion.Slerp(anchor.localRotation, tempRot, Time.deltaTime * loadout[currentIndex].aimSpeed);

        // anchor.rotation = Vector3.Lerp(anchor.rotation, SprintR.rotation, Time.deltaTime * loadout[currentIndex].aimSpeed);
        // crossHair.enabled = true;
        //}
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            //Hip
            //Quaternion tempRot = Quaternion.Euler(0f,0f,0f);
            //anchor.localRotation = Quaternion.Slerp(anchor.localRotation, tempRot, Time.deltaTime * loadout[currentIndex].aimSpeed);
            anchor.position = Vector3.Lerp(anchor.position, Hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            crossHair.enabled = true;
        }

    }


    void PlaySound(string ShotPath)
    {
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(path, GetComponent<Transform>().position);
        FMODUnity.RuntimeManager.PlayOneShotAttached(ShotPath, currentWeapon);

        //FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }

    void Shoot()
    {
        PlaySound(loadout[currentIndex].ShotPath);
        muzzleFlash.Play();

        Transform spawn = cam.transform;
        loadout[currentIndex].currentAmmo--;

        if (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
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
        for (int i = 0; i < 8; i++)
        {
            bloomShotty[i] = spawn.position + spawn.forward * 1000f;
            bloomShotty[i] += Random.Range(-adjustedBloom, adjustedBloom) * spawn.up;
            bloomShotty[i] += Random.Range(-adjustedBloom, adjustedBloom) * spawn.right;
            bloomShotty[i] -= spawn.position;
            bloomShotty[i].Normalize();
        }

        ///-----RECOIL-----/////
        if (Input.GetMouseButtonDown(0) || controller.state.Triggers.Right == 1 && controller.prevState.Triggers.Right < 1)
        {
            //tempTime = Time.time;
            //saveInitShot = Quaternion.Euler(cam.transform.localEulerAngles.x, 0f, 0f);
        }

        //Recoil Dampen
        timeFiringHeld = Time.time - tempTime;
        Quaternion maxRecoil = Quaternion.Euler(cam.transform.localEulerAngles.x + loadout[currentIndex].maxRecoil_x, 0f, 0f);
        cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, maxRecoil, Time.deltaTime * loadout[currentIndex].recoilSpeed * Mathf.Lerp(1, loadout[currentIndex].recoilDampen, timeFiringHeld));


        RaycastHit hitInfo = new RaycastHit();

        up_crosshair.transform.localPosition = new Vector3(up_crosshair.transform.localPosition.x, up_crosshair.transform.localPosition.y + 2, up_crosshair.transform.localPosition.z);
        down_crosshair.transform.localPosition = new Vector3(down_crosshair.transform.localPosition.x, down_crosshair.transform.localPosition.y - 2, down_crosshair.transform.localPosition.z);
        left_crosshair.transform.localPosition = new Vector3(left_crosshair.transform.localPosition.x - 2, left_crosshair.transform.localPosition.y, left_crosshair.transform.localPosition.z);
        right_crosshair.transform.localPosition = new Vector3(right_crosshair.transform.localPosition.x + 2, right_crosshair.transform.localPosition.y, right_crosshair.transform.localPosition.z);

        //bloom
        if (loadout[currentIndex].className == "Shotgun")
        {
            Target target;
            for (int j = 0; j < 8; j++)
            {
                Physics.Raycast(spawn.position, bloomShotty[j], out hitInfo, 100f);

                target = hitInfo.transform.GetComponent<Target>();

                if (target != null)
                {
                    if(hitInfo.collider.name == "Head")
                    {
                    StartCoroutine(displayHitmark());
                    target.takeDamage(loadout[currentIndex].damage);
                    }
                }

                //Shooting tasklist///////////
                if (hitInfo.collider.name == "Cube (8)" && _tutManager.Target1 == false)
                {
                    _tutManager.Target1 = true;
                }
                else if (hitInfo.collider.name == "Cube (6)" && _tutManager.Target2 == false)
                {
                    _tutManager.Target2 = true;
                }
                else if (hitInfo.collider.name == "Cube (4)" && _tutManager.Target3 == false)
                {
                    _tutManager.Target3 = true;
                }

                if (_tutManager.Target1 == true && _tutManager.Target2 == true && _tutManager.Target3 == true)
                {
                    if (!_tutManager.b_shootingComplete)
                        _tutManager.Notify("SHOOTING_COMPLETE");
                }

                if (hitInfo.collider.tag == "Wall")
                {
                    GameObject temp = _pool.GetBulletHole();
                    temp.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
                    temp.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                }

            }
        }
        else if (loadout[currentIndex].className != "Shotgun")
        {
            Physics.Raycast(spawn.position, bloom, out hitInfo, 100f);
            Target target = hitInfo.transform.GetComponent<Target>();
            Shatter Monkey = hitInfo.transform.GetComponent<Shatter>();

            if (target != null)
                {
                    if(hitInfo.collider.name == "Head")
                    {
                        StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[currentIndex].damage * 2);
                    }
                    else
                    {
                        StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[currentIndex].damage);
                    }
                }

            if (Monkey != null)
            {
                Monkey.shatterThis = true;
            }

            if (hitInfo.collider.tag == "Wall")
            {
                GameObject temp = _pool.GetBulletHole();
                temp.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
                temp.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }

        }


        //Shooting tasklist///////////
        if (hitInfo.collider.name == "Cube (8)" && _tutManager.Target1 == false)
        {
            _tutManager.Target1 = true;
        }
        else if (hitInfo.collider.name == "Cube (6)" && _tutManager.Target2 == false)
        {
            _tutManager.Target2 = true;
        }
        else if (hitInfo.collider.name == "Cube (4)" && _tutManager.Target3 == false)
        {
            _tutManager.Target3 = true;
        }

        if (_tutManager.Target1 == true && _tutManager.Target2 == true && _tutManager.Target3 == true)
        {
            if (!_tutManager.b_shootingComplete)
                _tutManager.Notify("SHOOTING_COMPLETE");
        }


        //GUN FX
        // currentWeapon.transform.Rotate(loadout[currentIndex].recoil, 0, 0);
        currentWeapon.transform.position -= -currentWeapon.transform.forward * loadout[currentIndex].kickBack;
        currentCool = loadout[currentIndex].firerate;

        _metricsLogger.shotsTaken++;
        if (loadout[currentIndex].currentAmmo == 0 && loadout[currentIndex].maxAmmo == 0 && loadout[currentIndex].ShotType == "Auto")
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Gun Effects/Dry Clip", currentWeapon);
    }

    IEnumerator displayHitmark()
    {
        _metricsLogger.shotsHit++;
        //Debug.Log(_metricsLogger.shotsHit);
        hitMark.enabled = true;

        yield return new WaitForSeconds(0.05f);
        hitMark.enabled = false;
    }


    private bool PlayerisReloading = false;
    void Reload()
    {

        Debug.Log(cam.transform.localRotation.eulerAngles.x + "||" + (saveInitShot.eulerAngles.x - 360));
        if (PlayerisReloading)
        {

            if (!origPosReset)
            {
                cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
                if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || controller.state.ThumbSticks.Right.Y != 0 || controller.state.ThumbSticks.Right.X != 0)
                {
                    //Debug.Log(origPosReset);

                    origPosReset = true;
                }
            }
            if (loadout[currentIndex].maxAmmo > 0)
            {
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
                reloadDelay += Time.deltaTime;
                Reloading.text = "Reloading..." + reloadDelay / loadout[currentIndex].reloadTime;
                //Debug.Log("Reloading..." + reloadDelay / loadout[currentIndex].reloadTime);
                if (reloadCancel)
                {
                    Debug.Log("Reload Cancelled");
                    reloadDelay = 0.0f;
                    reloadCancel = false;
                    PlayerisReloading = false;
                    Reloading.text = " ";
                    tempTime = Time.time;
                    return;
                }
                if (reloadDelay >= loadout[currentIndex].reloadTime)
                {
                    Debug.Log("Reload Finished");
                    int tempAmmoNeeded = (loadout[currentIndex].clipSize - loadout[currentIndex].currentAmmo);
                    int tempReloadAmmo = loadout[currentIndex].maxAmmo - (loadout[currentIndex].clipSize - loadout[currentIndex].currentAmmo);
                    if (loadout[currentIndex].maxAmmo >= tempAmmoNeeded)
                    {
                        loadout[currentIndex].maxAmmo = tempReloadAmmo;
                        loadout[currentIndex].currentAmmo += tempAmmoNeeded;
                    }
                    else
                    {
                        loadout[currentIndex].currentAmmo += loadout[currentIndex].maxAmmo;
                        loadout[currentIndex].maxAmmo = 0;
                    }
                    PlayerisReloading = false;
                    Reloading.text = " ";

                    tempTime = Time.time;

                    // Tutorial completion check
                    if (!_tutManager.b_reloadComplete)
                        _tutManager.Notify("RELOAD_COMPLETE");
                }
            }
            else
            {
                PlayerisReloading = false;
            }
        }
    }

    void SwitchWeapon()
    {
        checkWeapon = new RaycastHit();

        //if we hit something
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out checkWeapon, 3f))
        {
            //if it is tagged as a weapon
            if (checkWeapon.collider.tag == "Weapon")
            {
                PickUp.enabled = true;
                if(controller.state.IsConnected)
                {
                    PickUp.text = "Press X to pick up " + checkWeapon.collider.name;
                }
                else
                {
                    PickUp.text = "Press E to pick up " + checkWeapon.collider.name;
                }
                //if the user presses E
                if ((controller.state.Buttons.X == ButtonState.Pressed && controller.prevState.Buttons.X == ButtonState.Released) || Input.GetKeyDown(KeyCode.E) && currentIndex == 0)
                {
                    reloadCancel = true;
                    if (checkWeapon.collider.name == "Revolver")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;

                        if (loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                        }
                        else if (loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        }
                        else if (loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                            scriptOBJ[1].maxAmmo = scriptOBJ[1].alwaysMax;
                            scriptOBJ[1].currentAmmo = scriptOBJ[1].clipSize;
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
                    else if (checkWeapon.collider.name == "Tommy")
                    {

                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;
                        if (loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else if (loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                        }
                        else if (loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                            scriptOBJ[0].maxAmmo = scriptOBJ[0].alwaysMax;
                            scriptOBJ[0].currentAmmo = scriptOBJ[0].clipSize;
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
                        if (loadout[0].name == "Tommy")
                        {
                            tempMesh = gunMeshes[0];
                        }
                        else if (loadout[0].name == "Revolver")
                        {
                            tempMesh = gunMeshes[1];
                        }
                        else if (loadout[0].name == "MP40")
                        {
                            tempMesh = gunMeshes[2];
                            scriptOBJ[2].maxAmmo = scriptOBJ[2].alwaysMax;
                            scriptOBJ[2].currentAmmo = scriptOBJ[2].clipSize;
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
                            scriptOBJ[3].maxAmmo = scriptOBJ[3].alwaysMax;
                            scriptOBJ[3].currentAmmo = scriptOBJ[3].clipSize;
                        }

                        GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                        switched.name = loadout[0].name;
                        Destroy(checkWeapon.collider.gameObject);


                        loadout[0] = scriptOBJ[3];
                        Equip(0);

                        // Tutorial completion check
                        if (!_tutManager.b_swapComplete)
                            _tutManager.Notify("SWAP_COMPLETE");

                    }
                }
            }
        }
    }

    void getShootDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!PlayerisReloading && currentCool <= 0 && loadout[currentIndex].ShotType == "Single" && loadout[currentIndex].currentAmmo > 0)
                {
                    // Call your event function here.
                    origPosReset = false;
                    Shoot();
                }
        }
        if (controller.state.Triggers.Right == 1)
        {
            if (m_isAxisInUseDown == false)
            {
                tempTime = Time.time;
                saveInitShot = Quaternion.Euler(cam.transform.localEulerAngles.x, 0f, 0f);
                if (loadout[currentIndex].currentAmmo == 0 && loadout[currentIndex].maxAmmo == 0)
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Gun Effects/Dry Clip", currentWeapon);

                if (!PlayerisReloading && currentCool <= 0 && loadout[currentIndex].ShotType == "Single" && loadout[currentIndex].currentAmmo > 0)
                {
                    // Call your event function here.
                    origPosReset = false;
                    Shoot();
                }
                m_isAxisInUseDown = true;
            }
        }
        if (controller.state.Triggers.Right < 1)
        {
            m_isAxisInUseDown = false;
        }
    }

    void getShootUp()
    {
        if (controller.state.Triggers.Right == 0)
        {
            if (!m_isAxisInUseUp)
            {
                m_isAxisInUseUp = true;
            }
            if (!origPosReset)
            {
                if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || controller.state.ThumbSticks.Right.Y != 0 || controller.state.ThumbSticks.Right.X != 0)
                {
                    //Debug.Log(origPosReset);
                    origPosReset = true;
                    return;
                }
                cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
            }
        }
        if (controller.state.Triggers.Right > 0)
        {
            m_isAxisInUseUp = false;
        }
    }

    Rigidbody rb_Grenade;
    bool isNewNade = true;
    void throwGrenade()
    {
        reloadCancel = true;
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
    void MeshSwitch()
    {
        if (loadout[currentIndex].name == "M1911")
        {
            RobertoM1911.transform.position = this.transform.position;
            RobertoM1911.transform.rotation = this.transform.rotation;

            RobertoTommy.transform.position = new Vector3(45f, -2f, 0f);
            RobertoShotgun.transform.position = new Vector3(45f, -2f, 0f);
            RobertoMP40.transform.position = new Vector3(45f, -2f, 0f);
            RobertoRevolver.transform.position = new Vector3(45f, -2f, 0f);
        }
        else if (loadout[currentIndex].name == "Tommy")
        {
            RobertoTommy.transform.position = this.transform.position;
            RobertoTommy.transform.rotation = this.transform.rotation;

            RobertoM1911.transform.position = new Vector3(45f, -2f, 0f);
            RobertoShotgun.transform.position = new Vector3(45f, -2f, 0f);
            RobertoMP40.transform.position = new Vector3(45f, -2f, 0f);
            RobertoRevolver.transform.position = new Vector3(45f, -2f, 0f);
        }
        else if (loadout[currentIndex].name == "Revolver")
        {
            RobertoRevolver.transform.position = this.transform.position;
            RobertoRevolver.transform.rotation = this.transform.rotation;

            RobertoTommy.transform.position = new Vector3(45f, -2f, 0f);
            RobertoShotgun.transform.position = new Vector3(45f, -2f, 0f);
            RobertoMP40.transform.position = new Vector3(45f, -2f, 0f);
            RobertoM1911.transform.position = new Vector3(45f, -2f, 0f);
        }
        else if (loadout[currentIndex].name == "MP40")
        {
            RobertoMP40.transform.position = this.transform.position;
            RobertoMP40.transform.rotation = this.transform.rotation;

            RobertoTommy.transform.position = new Vector3(45f, -2f, 0f);
            RobertoShotgun.transform.position = new Vector3(45f, -2f, 0f);
            RobertoM1911.transform.position = new Vector3(45f, -2f, 0f);
            RobertoRevolver.transform.position = new Vector3(45f, -2f, 0f);
        }
        else if (loadout[currentIndex].name == "Shotgun")
        {
            RobertoShotgun.transform.position = this.transform.position;
            RobertoShotgun.transform.rotation = this.transform.rotation;

            RobertoTommy.transform.position = new Vector3(45f, -2f, 0f);
            RobertoM1911.transform.position = new Vector3(45f, -2f, 0f);
            RobertoMP40.transform.position = new Vector3(45f, -2f, 0f);
            RobertoRevolver.transform.position = new Vector3(45f, -2f, 0f);
        }
    }

}


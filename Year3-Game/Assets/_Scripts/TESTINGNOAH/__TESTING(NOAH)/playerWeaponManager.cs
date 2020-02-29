using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponManager : MonoBehaviour
{
    enum weaponID
    {
        M1911,
        TommyGun,
        MP40,
        Shotgun,
        Revolver
    }


    //Imported Scripts
    private PoolManager _pool;
    private playerInputManager inputManager;
    private dynamicCrosshair crosshair;

    [Header("Gun Parameters")]
    private Transform anchor;
    private Transform ADS;
    private Transform Hip;
    private float adjustedBloom;
    [SerializeField] public RaycastHit hitInfo;

    [Header("Reload")]
    private bool PlayerisReloading = false;
    private bool reloadCancel = false;
    private float reloadDelay = 0f;
    private float currentCool = 0f;

    [Header("Recoil")]
    private float currentRecoil;
    private float tempTime;
    private bool origPosReset = true;
    private Quaternion saveInitShot;
    private float timeFiringHeld;

    [Header("Weapon")]
    [SerializeField] public int selectedWeapon = 0;    // 0-Primary    // 1-Secondary
    [SerializeField] private int previousWeapon = 0;
    [SerializeField] public List<weaponInfo> loadout;
    [SerializeField] private GameObject currentWeapon;

    [Header("Camera")]
    [SerializeField] private Camera fpsCam;
    private float baseFOV;
    [SerializeField] private float FOVmod = 0.90f;



    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInParent<playerInputManager>();
        crosshair = GetComponentInParent<dynamicCrosshair>();
        baseFOV = fpsCam.fieldOfView;
        initLoadout();
        checkWeaponChange();
    }

    // Update is called once per frame
    void Update()
    {
        checkWeaponChange();
        checkWeaponGrab();
        Reload();


        if (currentWeapon != null)
        {
            Aim();
            getShootDown();
            if (!PlayerisReloading && inputManager.GetFireInputHeld() && currentCool <= 0 && loadout[selectedWeapon].gun.ShotType == "Auto" && loadout[selectedWeapon].currentAmmo > 0)
            {
                origPosReset = false;
                Shoot();
            }
            // Return back to original left click position
            if (!inputManager.GetFireInputHeld() && origPosReset == false)
            {
                fpsCam.transform.localRotation = Quaternion.Slerp(fpsCam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[selectedWeapon].gun.recoilSpeed);
                if (Mathf.Abs(fpsCam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || inputManager.GetLookInputsHorizontal() != 0 || inputManager.GetLookInputsVertical() != 0)
                {
                    origPosReset = true;
                }
            }
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4);
        }
        if (currentCool > 0)
        {
            currentCool -= Time.deltaTime;
        }
    }

    void initLoadout()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            loadout.Add(transform.GetChild(i).GetComponent<weaponInfo>());
            loadout[i].currentAmmo = loadout[i].gun.clipSize;
            loadout[i].maxAmmo = loadout[i].gun.alwaysMax;
            loadout[i].gun.isReloading = false;
        }
        currentWeapon = transform.GetChild(0).gameObject;
    }

    #region Weapon Actions
    void Aim()
    {
        anchor = currentWeapon.transform.Find("Anchor");
        ADS = currentWeapon.transform.Find("States/ADS");
        Hip = currentWeapon.transform.Find("States/Hip");
        if (inputManager.GetAimInputHeld())
        {
            //ADS
            crosshair.enabled = false;
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
            anchor.position = Vector3.Lerp(anchor.position, ADS.position, Time.deltaTime * loadout[selectedWeapon].gun.aimSpeed);
        }
        else
        {
            crosshair.enabled = true;
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            anchor.position = Vector3.Lerp(anchor.position, Hip.position, Time.deltaTime * loadout[selectedWeapon].gun.aimSpeed);
        }

    }


    void getShootDown()
    {
        if (inputManager.GetFireInputDown())
        {
            if (!PlayerisReloading && currentCool <= 0 && loadout[selectedWeapon].gun.ShotType == "Single" && loadout[selectedWeapon].currentAmmo > 0)
            {
                // Call your event function here.
                origPosReset = false;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        //PlaySound(loadout[currentIndex].ShotPath);
        Transform spawn = fpsCam.transform;
        loadout[selectedWeapon].currentAmmo--;

        if (inputManager.GetAimInputHeld())
        {
            adjustedBloom = loadout[selectedWeapon].gun.bloom / 3;
        }
        else
        {
            adjustedBloom = loadout[selectedWeapon].gun.bloom;
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
        if (inputManager.GetFireInputDown())
        {
            tempTime = Time.time;
            saveInitShot = Quaternion.Euler(fpsCam.transform.localEulerAngles.x, 0f, 0f);
        }

        //Recoil Dampen
        timeFiringHeld = Time.time - tempTime;
        Quaternion maxRecoil = Quaternion.Euler(fpsCam.transform.localEulerAngles.x + loadout[selectedWeapon].gun.maxRecoil_x, 0f, 0f);
        fpsCam.transform.localRotation = Quaternion.Slerp(fpsCam.transform.localRotation, maxRecoil, Time.deltaTime * loadout[selectedWeapon].gun.recoilSpeed * Mathf.Lerp(1, loadout[selectedWeapon].gun.recoilDampen, timeFiringHeld));

        hitInfo = new RaycastHit();
        //bloom
        if (loadout[selectedWeapon].gun.className == "Shotgun")
        {
            Target target;
            for (int j = 0; j < 8; j++)
            {
                Physics.Raycast(spawn.position, bloomShotty[j], out hitInfo, 100f);

                target = hitInfo.transform.GetComponent<Target>();

                if (target != null)
                {
                    if (hitInfo.collider.name == "Head")
                    {
                        //StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[selectedWeapon].gun.damage);
                    }
                }

                if (hitInfo.collider.tag == "Wall")
                {
                    GameObject temp = _pool.GetBulletHole();
                    temp.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
                    temp.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                }

            }
        }
        else if (loadout[selectedWeapon].gun.className != "Shotgun")
        {
            Physics.Raycast(spawn.position, bloom, out hitInfo, 100f);

            if (hitInfo.collider != null)
            {
                Target target = hitInfo.transform.GetComponent<Target>();
                Shatter Monkey = hitInfo.transform.GetComponent<Shatter>();

                if (target != null)
                {
                    if (hitInfo.collider.name == "Head")
                    {
                        //StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[selectedWeapon].gun.damage * 2);
                    }
                    else
                    {
                        //StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[selectedWeapon].gun.damage);
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
        }
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[selectedWeapon].gun.kickBack;
        currentCool = loadout[selectedWeapon].gun.firerate;
    }

    void Reload()
    {
        if (PlayerisReloading)
        {
            if (!origPosReset)
            {
                fpsCam.transform.localRotation = Quaternion.Slerp(fpsCam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[selectedWeapon].gun.recoilSpeed);
                if (Mathf.Abs(fpsCam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || inputManager.GetLookInputsVertical() != 0 || inputManager.GetLookInputsHorizontal() != 0)
                {
                    origPosReset = true;
                }
            }
            if (loadout[selectedWeapon].maxAmmo > 0)
            {
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
                reloadDelay += Time.deltaTime;
                if (reloadCancel)
                {
                    reloadDelay = 0.0f;
                    reloadCancel = false;
                    PlayerisReloading = false;
                    tempTime = Time.time;
                    return;
                }
                if (reloadDelay >= loadout[selectedWeapon].gun.reloadTime)
                {
                    int tempAmmoNeeded = (loadout[selectedWeapon].gun.clipSize - loadout[selectedWeapon].currentAmmo);
                    int tempReloadAmmo = loadout[selectedWeapon].maxAmmo - (loadout[selectedWeapon].gun.clipSize - loadout[selectedWeapon].currentAmmo);
                    if (loadout[selectedWeapon].maxAmmo >= tempAmmoNeeded)
                    {
                        loadout[selectedWeapon].maxAmmo = tempReloadAmmo;
                        loadout[selectedWeapon].currentAmmo += tempAmmoNeeded;
                    }
                    else
                    {
                        loadout[selectedWeapon].currentAmmo += loadout[selectedWeapon].maxAmmo;
                        loadout[selectedWeapon].maxAmmo = 0;
                    }
                    PlayerisReloading = false;

                    tempTime = Time.time;
                }
            }
            else
            {
                PlayerisReloading = false;
            }
        }
        else
        {
            if (loadout[selectedWeapon].currentAmmo == 0 && loadout[selectedWeapon].maxAmmo > 0)
            {
                reloadDelay = 0.0f;
                reloadCancel = false;
                PlayerisReloading = true;
                PlaySound(loadout[selectedWeapon].gun.ReloadPath);
            }

            if (inputManager.GetReloadInputDown())
            {
                reloadCancel = false;
                PlayerisReloading = true;
                reloadDelay = 0.0f;
                PlaySound(loadout[selectedWeapon].gun.ReloadPath);
            }
        }
    }

    #endregion

    #region Weapon Swapping
    //Check for weapon selection
    void checkWeaponChange()
    {
        previousWeapon = selectedWeapon;    //Save weapon held previous frame

        if (inputManager.GetSwitchWeaponInput() != 0)   //Check ScrollWheel
            cycleWeapon();
        if (inputManager.GetSelectWeaponInput() != 0 && inputManager.GetSelectWeaponInput() <= transform.childCount)   //Check Valid Alpha Keys
            selectWeapon();

        if (selectedWeapon != previousWeapon)
            weaponSwap();
    }

    //Check for Weapon Grab
    void checkWeaponGrab()
    {
        RaycastHit scanWeapon = new RaycastHit();

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out scanWeapon, 3f))
        {
            if (inputManager.GetInteractInputDown() && scanWeapon.collider.tag == "Weapon")
            {
                GameObject tempGrab = scanWeapon.collider.gameObject;
                if (transform.childCount < 2) //If only holding one weapon
                {
                    Instantiate(tempGrab.GetComponent<weaponInfo>().gun.weaponObj_Arms, transform.position, transform.rotation, transform);
                    loadout.Add(transform.GetChild(1).GetComponent<weaponInfo>());
                    Destroy(tempGrab); //destroy weapon 
                    selectedWeapon++;
                    weaponSwap(); //Update active weapon
                }
                else
                {
                    if (loadout[1].gun.weaponID != scanWeapon.collider.gameObject.GetComponent<weaponInfo>().gun.weaponID)
                    {
                        GameObject tempWall = Instantiate(loadout[1].gun.weaponObj, tempGrab.transform.position, tempGrab.transform.rotation); //Create gun to place on wall
                        tempWall.GetComponent<weaponInfo>().currentAmmo = loadout[1].currentAmmo; //Save current ammo to weapon on wall
                        tempWall.GetComponent<weaponInfo>().maxAmmo = loadout[1].maxAmmo;   //Save clip ammo to weapon on wall
                        loadout[1] = tempGrab.GetComponent<weaponInfo>();   //Get info of weapon being picked up
                        Destroy(tempGrab);  //Destroy being picked up
                        GameObject tempDestroy = transform.GetChild(1).gameObject;
                        tempDestroy.transform.SetParent(null);
                        Destroy(tempDestroy);  //Destroy current weapon being held

                        Instantiate(loadout[1].gun.weaponObj_Arms, transform.position, transform.rotation, transform); //Create new gun in primary slot based on wall info
                        weaponSwap(); //Update active weapon
                    }
                    else
                    {
                        loadout[1].maxAmmo = loadout[1].gun.alwaysMax + loadout[1].gun.clipSize - loadout[1].currentAmmo; //Refill Ammo.
                    }
                }
            }
        }
    }
    //Weapon Via Alpha Keys
    void selectWeapon()
    {
        selectedWeapon = inputManager.GetSelectWeaponInput() - 1;
    }

    //Weapon Via Scroll Wheel
    void cycleWeapon()
    {
        if (transform.childCount >= 2)
        {
            //LastWeapon -> FirstWeapon
            if (inputManager.GetSwitchWeaponInput() == -1)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }

            if (inputManager.GetSwitchWeaponInput() == 1)
            {
                //FirstWeapon -> LastWeapon
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }
        }
    }

    public void weaponSwap()
    {
        int i = 0;
        //Set selected weapon to active :: disable all others
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon.gameObject;
            }
            else
                weapon.gameObject.SetActive(false);

            i++; //next weapon
        }
    }
    #endregion

    #region Sounds
    void PlaySound(string ShotPath)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(ShotPath, currentWeapon);
    }
    #endregion
}

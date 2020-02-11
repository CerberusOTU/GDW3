using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class Weapon : MonoBehaviour
{
    //Imported Scripts
    private PoolManager _pool;
    private playerInputManager inputManager;
    private dynamicCrosshair crosshair;

    [Header("Gun Parameters")]
    private Transform anchor;
    private Transform ADS;
    private Transform Hip;
    public _Gun[] loadout;

    public _Gun[] scriptOBJ;
    //0 = Tommy
    //1 = Revolver
    //2 = MP40
    //3 = Shotgun
    private Transform weaponParent;


    public int currentIndex;

    private GameObject currentWeapon;

    //public Canvas hitMark;

    private Camera cam;
    private float baseFOV;
    private float FOVmod = 0.90f;


    //-----Recoil-----//
    [Header("Recoil")]
    private float currentRecoil;
    private float tempTime;
    private bool origPosReset = true;
    Quaternion saveInitShot;
    private float timeFiringHeld;

    //-----Reload-----//
    [Header("Reload")]
    private bool PlayerisReloading = false;
    private bool reloadCancel = false;
    private float reloadDelay = 0.0f;


    private float adjustedBloom;

    private float currentCool;

    private Motion player;

    Vector3 temp;
    Vector3 temp2;

    public ParticleSystem muzzleFlash;
    //to switch our gun meshes in scene
    bool isSwitched = false;
    RaycastHit checkWeapon;

    //Throwing Grenade
    private bool isCookingNade = false;
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    public int grenadeAmount = 2;

    void Start()
    {

        _pool = GameObject.FindObjectOfType<PoolManager>();
        inputManager = GetComponent<playerInputManager>();
        cam = GetComponentInChildren<Camera>();
        weaponParent = cam.gameObject.GetComponentInChildren<Transform>();
        crosshair = GetComponent<dynamicCrosshair>();
        //hitMark.enabled = false;
        baseFOV = cam.fieldOfView;

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

        loadout[0] = null;

        Equip(1);
    }

    void Update()
    {
        SwitchWeapon();
        Reload();

        float d = Input.GetAxis("Mouse ScrollWheel");

        if (grenadeAmount > 0)
        {
            if (Input.GetKey(KeyCode.G))
            {
                isCookingNade = true;
                throwGrenade();
            }
            //else if (Input.GetButtonUp("Grenade"))
            else if (Input.GetKeyUp(KeyCode.G))
            {
                if (grenadeAmount == 2)
                {
                    isCookingNade = false;
                    throwGrenade();
                    grenadeAmount--;
                }
                else if (grenadeAmount == 1)
                {
                    isCookingNade = false;
                    throwGrenade();
                    grenadeAmount--;
                }
            }
        }

        if (loadout[currentIndex] == loadout[0])
        {
            temp = transform.localScale;

            temp.x = 0.75f;
            temp.y = 0.75f;

        }

        if (loadout[currentIndex] == loadout[1])
        {
            temp = transform.localScale;

            temp.x = 0.75f;
            temp.y = 0.75f;

        }


        if (loadout[currentIndex].currentAmmo == 0 && loadout[currentIndex].maxAmmo > 0 && !PlayerisReloading)
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

        if ((Input.GetKeyDown(KeyCode.R) && !PlayerisReloading))
        {
            reloadCancel = false;
            PlayerisReloading = true;
            reloadDelay = 0.0f;
            PlaySound(loadout[currentIndex].ReloadPath);

        }

        //d > 0f is scrolling up
        if (loadout[0] != null)
        {
            if ((d > 0f && currentIndex != 0))
            {
                reloadCancel = true;
                Equip(0);
            }
            else if ((d < 0f && currentIndex != 1))
            {
                reloadCancel = true;
                Equip(1);
            }
        }


        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            getShootDown();

            if (!PlayerisReloading && (Input.GetMouseButton(0)) && currentCool <= 0 && loadout[currentIndex].ShotType == "Auto" && loadout[currentIndex].currentAmmo > 0)
            {
                origPosReset = false;
                Shoot();
            }
            // Return back to original left click position
            if (!Input.GetMouseButton(0) && origPosReset == false)
            {
                cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
                if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    Debug.Log(origPosReset);
                    origPosReset = true;
                }
            }
        }

        if (currentCool > 0)
        {
            currentCool -= Time.deltaTime;
        }
    }

    public void Equip(int _ind)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        currentIndex = _ind;
        GameObject newWeapon = Instantiate(loadout[_ind].weaponObj_Arms, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = newWeapon;
    }

    void Aim(bool isAiming)
    {
        anchor = currentWeapon.transform.Find("Anchor");
        ADS = currentWeapon.transform.Find("States/ADS");
        Hip = currentWeapon.transform.Find("States/Hip");
        if (isAiming && !inputManager.GetSprintInputHeld())
        {
            //ADS
            crosshair.enabled = false;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
            anchor.position = Vector3.Lerp(anchor.position, ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            crosshair.enabled = true;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            anchor.position = Vector3.Lerp(anchor.position, Hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }

    }

    void PlaySound(string ShotPath)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(ShotPath, currentWeapon);
    }

    void Shoot()
    {
        PlaySound(loadout[currentIndex].ShotPath);
        muzzleFlash.Play();

        Transform spawn = cam.transform;
        loadout[currentIndex].currentAmmo--;

        if (Input.GetMouseButton(1))
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
        if (Input.GetMouseButtonDown(0))
        {
            tempTime = Time.time;
            saveInitShot = Quaternion.Euler(cam.transform.localEulerAngles.x, 0f, 0f);
        }

        //Recoil Dampen
        timeFiringHeld = Time.time - tempTime;
        Quaternion maxRecoil = Quaternion.Euler(cam.transform.localEulerAngles.x + loadout[currentIndex].maxRecoil_x, 0f, 0f);
        cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, maxRecoil, Time.deltaTime * loadout[currentIndex].recoilSpeed * Mathf.Lerp(1, loadout[currentIndex].recoilDampen, timeFiringHeld));

        RaycastHit hitInfo = new RaycastHit();

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
                    if (hitInfo.collider.name == "Head")
                    {
                        //StartCoroutine(displayHitmark());
                        target.takeDamage(loadout[currentIndex].damage);
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
        else if (loadout[currentIndex].className != "Shotgun")
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
                        target.takeDamage(loadout[currentIndex].damage * 2);
                    }
                    else
                    {
                        //StartCoroutine(displayHitmark());
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
        }

        //GUN FX
        // currentWeapon.transform.Rotate(loadout[currentIndex].recoil, 0, 0);
        currentWeapon.transform.position -= -currentWeapon.transform.forward * loadout[currentIndex].kickBack;
        currentCool = loadout[currentIndex].firerate;

        if (loadout[currentIndex].currentAmmo == 0 && loadout[currentIndex].maxAmmo == 0 && loadout[currentIndex].ShotType == "Auto")
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Gun Effects/Dry Clip", currentWeapon);
    }

    IEnumerator displayHitmark()
    {
        //hitMark.enabled = true;
        yield return new WaitForSeconds(0.05f);
        //hitMark.enabled = false;
    }



    void Reload()
    {
        if (PlayerisReloading)
        {
            if (!origPosReset)
            {
                cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, saveInitShot, Time.deltaTime * loadout[currentIndex].recoilSpeed);
                if (Mathf.Abs(cam.transform.localEulerAngles.x - saveInitShot.eulerAngles.x) <= 0.1f || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    origPosReset = true;
                }
            }
            if (loadout[currentIndex].maxAmmo > 0)
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
                if (reloadDelay >= loadout[currentIndex].reloadTime)
                {
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

                    tempTime = Time.time;
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
                //if the user presses E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    reloadCancel = true;
                    if (checkWeapon.collider.name == "Revolver")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;

                        if (loadout[0] != null)
                        {
                            if (loadout[0].name == "Tommy")
                            {
                                tempMesh = scriptOBJ[0].weaponObj;
                            }
                            else if (loadout[0].name == "MP40")
                            {
                                tempMesh = scriptOBJ[2].weaponObj;

                            }
                            else if (loadout[0].name == "Revolver")
                            {
                                tempMesh = scriptOBJ[1].weaponObj;

                                scriptOBJ[1].maxAmmo = scriptOBJ[1].alwaysMax;
                                scriptOBJ[1].currentAmmo = scriptOBJ[1].clipSize;
                            }
                            else if (loadout[0].name == "Shotgun")
                            {
                                tempMesh = scriptOBJ[3].weaponObj;

                            }
                        }
                        else
                        {
                            loadout[0].name = "Revolver";

                            scriptOBJ[1].maxAmmo = scriptOBJ[1].alwaysMax;
                            scriptOBJ[1].currentAmmo = scriptOBJ[1].clipSize;


                            Destroy(checkWeapon.collider.gameObject);
                            Equip(0);
                        }

                        if (loadout[0] != null)
                        {
                            GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                            switched.name = loadout[0].name;
                        }

                        Destroy(checkWeapon.collider.gameObject);


                        loadout[0] = scriptOBJ[1];
                        Equip(0);
                    }
                    else if (checkWeapon.collider.name == "Tommy")
                    {

                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;
                        if (loadout[0] != null)
                        {
                            if (loadout[0].name == "Revolver")
                            {
                                tempMesh = scriptOBJ[1].weaponObj;
                            }
                            else if (loadout[0].name == "MP40")
                            {
                                tempMesh = scriptOBJ[2].weaponObj;
                            }
                            else if (loadout[0].name == "Tommy")
                            {
                                tempMesh = scriptOBJ[0].weaponObj;
                                scriptOBJ[0].maxAmmo = scriptOBJ[0].alwaysMax;
                                scriptOBJ[0].currentAmmo = scriptOBJ[0].clipSize;
                            }
                            else if (loadout[0].name == "Shotgun")
                            {
                                tempMesh = scriptOBJ[3].weaponObj;
                            }
                        }
                        else
                        {
                            loadout[0].name = "Tommy";

                            scriptOBJ[0].maxAmmo = scriptOBJ[0].alwaysMax;
                            scriptOBJ[0].currentAmmo = scriptOBJ[0].clipSize;

                            Destroy(checkWeapon.collider.gameObject);
                            Equip(0);

                        }

                        if (loadout[0] != null)
                        {
                            GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                            switched.name = loadout[0].name;
                        }

                        Destroy(checkWeapon.collider.gameObject);


                        loadout[0] = scriptOBJ[0];
                        Equip(0);
                    }
                    else if (checkWeapon.collider.name == "MP40")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;
                        if (loadout[0] != null)
                        {

                            if (loadout[0].name == "Tommy")
                            {
                                tempMesh = scriptOBJ[0].weaponObj;
                            }
                            else if (loadout[0].name == "Revolver")
                            {
                                tempMesh = scriptOBJ[1].weaponObj;
                            }
                            else if (loadout[0].name == "MP40")
                            {
                                tempMesh = scriptOBJ[2].weaponObj;
                                scriptOBJ[2].maxAmmo = scriptOBJ[2].alwaysMax;
                                scriptOBJ[2].currentAmmo = scriptOBJ[2].clipSize;
                            }
                            else if (loadout[0].name == "Shotgun")
                            {
                                tempMesh = scriptOBJ[3].weaponObj;
                            }
                        }
                        else
                        {
                            loadout[0].name = "MP40";

                            scriptOBJ[2].maxAmmo = scriptOBJ[2].alwaysMax;
                            scriptOBJ[2].currentAmmo = scriptOBJ[2].clipSize;

                            Destroy(checkWeapon.collider.gameObject);
                            Equip(0);

                        }

                        if (loadout[0] != null)
                        {
                            GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                            switched.name = loadout[0].name;

                        }
                        Destroy(checkWeapon.collider.gameObject);
                        loadout[0] = scriptOBJ[2];
                        Equip(0);

                    }
                    else if (checkWeapon.collider.name == "Shotgun")
                    {
                        Transform temp = checkWeapon.collider.GetComponent<Transform>();

                        GameObject tempMesh = null;

                        if (loadout[0] != null)
                        {
                            if (loadout[0].name == "MP40")
                            {
                                tempMesh = scriptOBJ[2].weaponObj;
                            }
                            else if (loadout[0].name == "Revolver")
                            {
                                tempMesh = scriptOBJ[1].weaponObj;
                            }
                            else if (loadout[0].name == "Tommy")
                            {
                                tempMesh = scriptOBJ[0].weaponObj;
                            }
                            else if (loadout[0].name == "Shotgun")
                            {
                                tempMesh = scriptOBJ[3].weaponObj;
                                scriptOBJ[3].maxAmmo = scriptOBJ[3].alwaysMax;
                                scriptOBJ[3].currentAmmo = scriptOBJ[3].clipSize;

                            }
                        }
                        else
                        {
                            loadout[0].name = "Shotgun";

                            scriptOBJ[3].maxAmmo = scriptOBJ[3].alwaysMax;
                            scriptOBJ[3].currentAmmo = scriptOBJ[3].clipSize;

                            Destroy(checkWeapon.collider.gameObject);
                            Equip(0);

                        }

                        if (loadout[0] != null)
                        {
                            GameObject switched = Instantiate(tempMesh, temp.position, temp.rotation) as GameObject;
                            switched.name = loadout[0].name;

                        }
                        Destroy(checkWeapon.collider.gameObject);
                        loadout[0] = scriptOBJ[3];
                        Equip(0);
                    }
                }
            }
        }
    }

    void getShootDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!PlayerisReloading && currentCool <= 0 && loadout[currentIndex].ShotType == "Single" && loadout[currentIndex].currentAmmo > 0)
            {
                // Call your event function here.
                origPosReset = false;
                Shoot();
            }
        }
    }

    //Grenade Variables
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
}


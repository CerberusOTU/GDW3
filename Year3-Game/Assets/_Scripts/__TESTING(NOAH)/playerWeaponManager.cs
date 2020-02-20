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

    private playerInputManager inputManager;

    [Header("Weapon")]
    [SerializeField] private int selectedWeapon = 0;    // 0-Primary    // 1-Secondary
    [SerializeField] private int previousWeapon = 0;
    [SerializeField] private List<_Gun> loadout;
    [SerializeField] private GameObject currentWeapon;

    [Header("Camera")]
    [SerializeField] private Camera fpsCam;



    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInParent<playerInputManager>();
        fpsCam = transform.GetComponent<Camera>();
        initLoadout();
        checkWeaponChange();
    }

    // Update is called once per frame
    void Update()
    {
        checkWeaponChange();
        checkWeaponGrab();
    }

    void initLoadout()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            loadout.Add(transform.GetChild(i).GetComponent<weaponInfo>().gun);
        }
    }

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
                if (transform.childCount < 2)
                {
                    loadout.Add(scanWeapon.collider.gameObject.GetComponent<weaponInfo>().gun);
                    Instantiate(loadout[1].weaponObj_Arms, transform.position, transform.rotation, transform);
                }
                else
                {
                    if (loadout[1].weaponID != scanWeapon.collider.gameObject.GetComponent<weaponInfo>().gun.weaponID)
                    {
                        GameObject temp = Instantiate(loadout[1].weaponObj, scanWeapon.collider.transform);
                        temp.GetComponent<weaponInfo>().gun = loadout[1];
                        loadout[1] = scanWeapon.collider.gameObject.GetComponent<weaponInfo>().gun;
                        Destroy(transform.GetChild(1));
                        Instantiate(loadout[1].weaponObj_Arms, transform.position, transform.rotation, transform);
                    }
                    else
                    {
                        loadout[1].currentAmmo = loadout[1].maxAmmo;
                    }
                }
            }
        }
    }
    //Weapon Via Alpha Keys
    void selectWeapon()
    {
        Debug.Log(inputManager.GetSelectWeaponInput());
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

    void weaponSwap()
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
}

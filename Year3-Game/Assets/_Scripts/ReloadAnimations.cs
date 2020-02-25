using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    private playerWeaponManager weaponScript;
    bool m_reload;
    int ammoMissing;


    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        weaponScript = GameObject.FindObjectOfType<playerWeaponManager>();
        
        m_reload = false;
    }

    void Update()
    {
        ammoMissing = weaponScript.loadout[1].gun.clipSize - weaponScript.loadout[1].currentAmmo;
        m_Animator.SetInteger("MissingAmmo", ammoMissing);

        Debug.Log("Missing Ammo: " + ammoMissing);    



        if (Input.GetKeyDown(KeyCode.R) && weaponScript.loadout[1].currentAmmo != weaponScript.loadout[0].gun.clipSize
            && weaponScript.selectedWeapon == 0)
        {
            m_reload = true;
            Debug.Log("XXReloadPistol ");
            m_Animator.SetBool("M1911Reload", true);

        }


        if (weaponScript.loadout[1] != null && Input.GetKeyDown(KeyCode.R) && (weaponScript.loadout[1].currentAmmo != weaponScript.loadout[1].gun.clipSize) && weaponScript.selectedWeapon == 0)
        {
            m_reload = true;
            if (weaponScript.loadout[1].name == "Tommy")
            {
                m_Animator.SetBool("tommyReload", true);
                //Debug.Log("Tommy Reloading");
            }
            if (weaponScript.loadout[1].name == "MP40")
            {
                m_Animator.SetBool("MP40Reloading", true);
                //Debug.Log("MP40 Reloading");
            }
            if (weaponScript.loadout[1].name == "Shotgun")
            {
                weaponScript.loadout[1].gun.reloadTime += ammoMissing*0.583f;
                //m_Animator.SetBool("ShotgunEndReload", false);
                m_Animator.SetBool("ShotgunStartReload", true);
                //Debug.Log("Shotgun Start Reload");
            }

            //Debug.Log("XXReloadMain");
        }

        if (ammoMissing == 0)
        {
            Debug.Log("Shotgun End Reload");

                m_Animator.SetBool("ShotgunStartReload", false);
                m_Animator.SetBool("ShotgunReloading", false);
                m_Animator.SetBool("ShotgunEndReload", true);
                weaponScript.loadout[1].gun.reloadTime = 1.833f;            
        }

        //if (weaponScript.currentIndex[0].currentAmmo < weaponScript.currentIndex[0].maxAmmo);
       // {
            
        //}

        if ((weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].gun.clipSize) && (m_Animator.GetBool("M1911Reload") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadPistolFinish");
            m_Animator.SetBool("M1911Reload", false);
        }

        //if ((weaponScript.loadout[1].currentAmmo == weaponScript.loadout[1].clipSize) && (m_Animator.GetBool("tommyReload") == true))
        //{
        //    m_reload = false;
        //    m_Animator.SetBool("tommyReload", false);         
        //}

        //if ((weaponScript.loadout[1].currentAmmo == weaponScript.loadout[1].clipSize) && (m_Animator.GetBool("MP40Reloading") == true))
        //{
        //    m_reload = false;
        //    m_Animator.SetBool("MP40Reloading", false); 
        //    Debug.Log("MP40 Reloading = fALSE");           
        //}


        if (weaponScript.loadout[1] != null && (weaponScript.loadout[1].currentAmmo == weaponScript.loadout[1].gun.clipSize)
            && (m_Animator.GetBool("tommyReload") == true) || (m_Animator.GetBool("MP40Reloading") == true) || (m_Animator.GetBool("ShotgunReloading") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");
            if (weaponScript.loadout[1].name == "Tommy")
                m_Animator.SetBool("tommyReload", false);
            if (weaponScript.loadout[1].name == "MP40")
                m_Animator.SetBool("MP40Reloading", false);
            /*if (weaponScript.loadout[0].name == "Shotgun")
            {
                 Debug.Log("Shotgun End Reload");

                m_Animator.SetBool("ShotgunStartReload", false);
                m_Animator.SetBool("ShotgunReloading", false);
                //m_Animator.SetBool("ShotgunEndReload", true);
            }   */             
        }

        if (weaponScript.loadout[0].currentAmmo == 0 && weaponScript.loadout[0].maxAmmo > 0)
        {
            m_reload = true;
            Debug.Log("XXAutoReloadPistol");
            m_Animator.SetBool("M1911Reload", true);

        }

        if (weaponScript.loadout[1] != null && (weaponScript.loadout[1].currentAmmo == 0 && weaponScript.loadout[1].maxAmmo > 0))
        {
            m_reload = true;
            Debug.Log("XXAutoReloadMain");
            if (weaponScript.loadout[1].name == "Tommy")
                m_Animator.SetBool("tommyReload", true);
            if (weaponScript.loadout[1].name == "MP40")
                m_Animator.SetBool("MP40Reloading", true);
            if (weaponScript.loadout[1].name == "Shotgun")
            {
                m_Animator.SetBool("ShotgunStartReload", true);  
                weaponScript.loadout[1].gun.reloadTime += ammoMissing*0.583f;
            }                  
        }

    }

    public void ShellIn()
    {
    Debug.Log("Shotgun ShellIn");
    if(weaponScript.loadout[1].currentAmmo < weaponScript.loadout[1].gun.clipSize)
    weaponScript.loadout[1].currentAmmo += 1;
    }

}


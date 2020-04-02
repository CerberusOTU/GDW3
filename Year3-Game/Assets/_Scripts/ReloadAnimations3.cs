using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations3 : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    private Weapon3 weaponScript;
    bool m_reload;
    int ammoMissing;


    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        weaponScript = GameObject.FindObjectOfType<Weapon3>();

        m_reload = false;
    }

    void Update()
    {
        if (weaponScript.loadout[0].maxAmmo != 0)
            ammoMissing = weaponScript.loadout[0].clipSize - weaponScript.loadout[0].currentAmmo;
        else
            ammoMissing = 0;

        //if (m_reload == true)
        //    Debug.Log("Shotgun reload true");
        //else if (m_reload == false)
        //    Debug.Log("Shotgun reload false");

        m_Animator.SetInteger("MissingAmmo", ammoMissing);

        Debug.Log("Missing Ammo: " + ammoMissing);


        if (weaponScript.PlayerisReloading == true && weaponScript.loadout[1].currentAmmo != weaponScript.loadout[1].clipSize
            && weaponScript.currentIndex == 1)
        {
            m_reload = true;
            Debug.Log("XXReloadPistol ");
            m_Animator.SetBool("M1911Reload", true);

        }


        if (weaponScript.loadout[0] != null && weaponScript.PlayerisReloading == true &&
            (weaponScript.loadout[0].currentAmmo != weaponScript.loadout[0].clipSize) &&
            weaponScript.loadout[0].maxAmmo >= 0 &&
            weaponScript.currentIndex == 0 &&
            m_reload == false)
        {
            m_reload = true;
            if (weaponScript.loadout[0].name == "Tommy")
            {
                m_Animator.SetBool("tommyReload", true);
                //Debug.Log("Tommy Reloading");
            }
            if (weaponScript.loadout[0].name == "Revolver")
            {
                m_Animator.SetBool("RevolverReload", true);
                Debug.Log("Revolver Reloading");
            }
            if (weaponScript.loadout[0].name == "MP40")
            {
                m_Animator.SetBool("MP40Reloading", true);
                //Debug.Log("MP40 Reloading");
            }
            if (weaponScript.loadout[0].name == "Shotgun")
            {
                weaponScript.loadout[0].reloadTime += ammoMissing * 0.583f;

                //m_Animator.SetBool("ShotgunEndReload", false);
                m_Animator.SetBool("ShotgunStartReload", true);
                Debug.Log("Shotgun Start Reload");
            }

            //Debug.Log("XXReloadMain");
        }

                Debug.Log("Shotgun Time " + weaponScript.loadout[0].reloadTime);

        if (weaponScript.PlayerisReloading == false || ammoMissing == 0)
        {
            Debug.Log("Shotgun End Reload");

            m_Animator.SetBool("ShotgunStartReload", false);
            m_Animator.SetBool("ShotgunReloading", false);
            m_Animator.SetBool("ShotgunEndReload", true);
            weaponScript.loadout[0].reloadTime = 1.833f;
            m_reload = false;
        }


        //if (weaponScript.currentIndex[0].currentAmmo < weaponScript.currentIndex[0].maxAmmo);
        // {

        //}

        if ((weaponScript.loadout[1].currentAmmo == weaponScript.loadout[1].clipSize) && (m_Animator.GetBool("M1911Reload") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadPistolFinish");
            m_Animator.SetBool("M1911Reload", false);
        }

        //if ((weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].clipSize) && (m_Animator.GetBool("tommyReload") == true))
        //{
        //    m_reload = false;
        //    m_Animator.SetBool("tommyReload", false);         
        //}

        //if ((weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].clipSize) && (m_Animator.GetBool("MP40Reloading") == true))
        //{
        //    m_reload = false;
        //    m_Animator.SetBool("MP40Reloading", false); 
        //    Debug.Log("MP40 Reloading = fALSE");           
        //}

        //Setting Back too false
        if (weaponScript.loadout[0] != null && (weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].clipSize)
            && (m_Animator.GetBool("tommyReload") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");
            if (weaponScript.loadout[0].name == "Tommy")
                m_Animator.SetBool("tommyReload", false);
        }

        if (weaponScript.loadout[0] != null && (weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].clipSize)
            && (m_Animator.GetBool("MP40Reloading") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");

            if (weaponScript.loadout[0].name == "MP40")
                m_Animator.SetBool("MP40Reloading", false);

        }

        if (weaponScript.loadout[0] != null && (weaponScript.loadout[0].currentAmmo == weaponScript.loadout[0].clipSize)
            && (m_Animator.GetBool("RevolverReload") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");

            if (weaponScript.loadout[0].name == "Revolver")
            {
                m_Animator.SetBool("RevolverReload", false);
                Debug.Log("Revolver Reload Finished");
            }
        }

        ////
        if (weaponScript.loadout[1].currentAmmo == 0 && weaponScript.loadout[1].maxAmmo > 0)
        {
            m_reload = true;
            Debug.Log("XXAutoReloadPistol");
            m_Animator.SetBool("M1911Reload", true);

        }

        if (weaponScript.loadout[0] != null && (weaponScript.loadout[0].currentAmmo == 0 && weaponScript.loadout[0].maxAmmo > 0))
        {
            m_reload = true;
            Debug.Log("XXAutoReloadMain");
            if (weaponScript.loadout[0].name == "Tommy")
                m_Animator.SetBool("tommyReload", true);
            if (weaponScript.loadout[0].name == "MP40")
                m_Animator.SetBool("MP40Reloading", true);
            if (weaponScript.loadout[0].name == "Revolver")
                m_Animator.SetBool("RevolverReload", true);
            if (weaponScript.loadout[0].name == "Shotgun")
            {
                m_Animator.SetBool("ShotgunStartReload", true);
                weaponScript.loadout[0].reloadTime += ammoMissing * 0.583f;
            }
        }

    }

    public void ShellIn()
    {

        if (weaponScript.loadout[0].currentAmmo < weaponScript.loadout[0].clipSize &&
        weaponScript.loadout[0].maxAmmo != 0)
        {
            Debug.Log("Shotgun ShellIn");

            //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Gun Reloads/ShotgunShellIn", weaponScript.currentWeapon);

            weaponScript.loadout[0].currentAmmo += 1;
            weaponScript.loadout[0].maxAmmo -= 1;

        }


    }

    public void Cock()
    {
        //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Gun Reloads/ShotgunCock", weaponScript.currentWeapon);
    }

}


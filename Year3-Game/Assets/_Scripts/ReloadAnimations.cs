using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    private playerWeaponManager weaponScript;
    // Use this for deciding if the GameObject can jump or not
    bool M1911Reload;
    bool TommyReload;
    bool m_reload;
    public GameObject Player;
    int counter1 = 0;
    int counter2 = 0;

    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        weaponScript = GameObject.FindObjectOfType<playerWeaponManager>();
        // The GameObject cannot jump
        M1911Reload = false;
        TommyReload = false;
        m_reload = false;
    }

    void Update()
    {
        Debug.Log("xx " + weaponScript.selectedWeapon);
        Debug.Log("Current Gun = " + weaponScript.loadout[weaponScript.selectedWeapon].name);

        if (Input.GetKeyDown(KeyCode.R) && weaponScript.loadout[1].currentAmmo != weaponScript.loadout[1].gun.clipSize
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
                Debug.Log("Tommy Reloading");
            }
            if (weaponScript.loadout[1].name == "MP40")
            {
                m_Animator.SetBool("MP40Reloading", true);
                Debug.Log("MP40 Reloading");
            }

            Debug.Log("XXReloadMain");
        }


        if ((weaponScript.loadout[1].currentAmmo == weaponScript.loadout[1].gun.clipSize) && (m_Animator.GetBool("M1911Reload") == true))
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
            && (m_Animator.GetBool("tommyReload") == true) || (m_Animator.GetBool("MP40Reloading") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");
            if (weaponScript.loadout[1].name == "Tommy")
                m_Animator.SetBool("tommyReload", false);
            if (weaponScript.loadout[1].name == "MP40")
                m_Animator.SetBool("MP40Reloading", false);
        }

        if (weaponScript.loadout[1].currentAmmo == 0 && weaponScript.loadout[1].maxAmmo > 0)
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
        }




        ///////////////////////////////Reloads//////////////////////////////////////

        //The GameObject is jumping, so send the Boolean as enabled to the Animator. The jump animation plays.
        if (m_Animator != null)// animator is of type "Animator"
        {

            if (m_Animator.runtimeAnimatorController != null)
            {
                if (m_reload == true)
                {
                    Debug.Log("animationRan");
                    //m_Animator.SetBool("M1911Reload", true);
                    //m_Animator.SetBool("tommyReload", true);
                    //m_Animator.SetBool("MP40Reloading", true);
                }
            }
        }

        if (m_Animator != null)// animator is of type "Animator"
        {

            if (m_Animator.runtimeAnimatorController != null)
            {
                if (m_reload == false)
                {
                    //m_Animator.SetBool("M1911Reload", false);
                    //m_Animator.SetBool("tommyReload", false);
                    //m_Animator.SetBool("MP40Reloading", false);
                }
            }
        }

    }

}


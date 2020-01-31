using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    private Weapon weaponScript;
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
        weaponScript = Player.GetComponent<Weapon>();
        // The GameObject cannot jump
        M1911Reload = false;
        TommyReload = false;
        m_reload = false;
    }

    void Update()
    {

        Debug.Log(Player.GetComponent<Weapon>().currentIndex);

        if (Input.GetKey(KeyCode.R) && Player.GetComponent<Weapon>().loadout[1].currentAmmo != Player.GetComponent<Weapon>().loadout[1].clipSize)
        {
            m_reload = true;
            Debug.Log("XXReloadPistol");
        }
        

        if (Player.GetComponent<Weapon>().loadout[0] != null && Input.GetKey(KeyCode.R) && Player.GetComponent<Weapon>().loadout[0].currentAmmo != Player.GetComponent<Weapon>().loadout[0].clipSize)
        {
            m_reload = true;
            Debug.Log("XXReloadMain");
        }









        if ((Player.GetComponent<Weapon>().loadout[1].currentAmmo == Player.GetComponent<Weapon>().loadout[1].clipSize) && (m_Animator.GetBool("M1911Reload") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadPistolFinish");
        }



        if (Player.GetComponent<Weapon>().loadout[0] != null && (Player.GetComponent<Weapon>().loadout[0].currentAmmo == Player.GetComponent<Weapon>().loadout[0].clipSize)
            && (m_Animator.GetBool("tommyReload") == true) || (m_Animator.GetBool("MP40Reloading") == true))
        {
            m_reload = false;
            Debug.Log("XXReloadMainFinish");
        }













        if (Player.GetComponent<Weapon>().loadout[1].currentAmmo == 0 && Player.GetComponent<Weapon>().loadout[1].maxAmmo > 0)
        {
            m_reload = true;
            Debug.Log("XXAutoReloadPistol");
        }

        if (Player.GetComponent<Weapon>().loadout[0] != null && (Player.GetComponent<Weapon>().loadout[0].currentAmmo == 0 && Player.GetComponent<Weapon>().loadout[0].maxAmmo > 0))
        {
            m_reload = true;
            Debug.Log("XXAutoReloadMain");
        }




        ///////////////////////////////Reloads//////////////////////////////////////

        //The GameObject is jumping, so send the Boolean as enabled to the Animator. The jump animation plays.
        if (m_Animator != null)// animator is of type "Animator"
        {

            if (m_Animator.runtimeAnimatorController != null)
            {
                if (m_reload == true)
                {
                    Debug.Log("animationran");
                    m_Animator.SetBool("M1911Reload", true);
                    m_Animator.SetBool("tommyReload", true);
                    m_Animator.SetBool("MP40Reloading", true);
                }
            }
        }

        if (m_Animator != null)// animator is of type "Animator"
        {

            if (m_Animator.runtimeAnimatorController != null)
            {
                if (m_reload == false)
                {
                    m_Animator.SetBool("M1911Reload", false);
                    m_Animator.SetBool("tommyReload", false);
                    m_Animator.SetBool("MP40Reloading", false);
                }
            }
        }

    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    private Weapon weaponScript;
    // Use this for deciding if the GameObject can jump or not
    bool m_Jump;
    public GameObject Player;
    int counter1 = 0;
    int counter2 = 0;

    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        weaponScript= Player.GetComponent<Weapon>();
        // The GameObject cannot jump
        m_Jump = false;
    }

    void Update()
    {
        if (m_Jump)
        Debug.Log("Playing Anim");
        if (!m_Jump)
        Debug.Log("Not Playing Anim");

        if (Player.GetComponent<Weapon>().PlayerisReloading == false && Input.GetKey(KeyCode.R) 
        && (Player.GetComponent<Weapon>().loadout[1].currentAmmo != 
        Player.GetComponent<Weapon>().loadout[1].clipSize))   
        {
            m_Jump = true;
            //Debug.Log("PLEASE");
        }



        if ((Player.GetComponent<Weapon>().loadout[1].currentAmmo == 
        Player.GetComponent<Weapon>().loadout[1].clipSize))
        {
            m_Jump = false;
            //Debug.Log("No thank you");
        
        }

        if (m_Animator != null)// animator is of type "Animator"
        {
            
            if(m_Animator.runtimeAnimatorController!=null)
            {
                if (m_Jump == false)
                {
                    //m_Animator.SetBool("tommyReload", false);
                    //m_Animator.SetBool("MP40Reloading", false);
                    m_Animator.SetBool("M1911Reload", false);
                }
            }
        }

        //The GameObject is jumping, so send the Boolean as enabled to the Animator. The jump animation plays.
        if (m_Animator != null)// animator is of type "Animator"
        {
            
            if(m_Animator.runtimeAnimatorController!=null)
            {
                if (m_Jump == true)
                {
                    //m_Animator.SetBool("tommyReload", true);
                    //m_Animator.SetBool("MP40Reloading", true);
                    m_Animator.SetBool("M1911Reload", true);                
                }
            }
        }


    }
    
}
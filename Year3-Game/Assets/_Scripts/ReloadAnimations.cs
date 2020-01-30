using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimations : MonoBehaviour
{
    //Fetch the Animator
    public Animator m_Animator;
    // Use this for deciding if the GameObject can jump or not
    bool m_Jump;
    int counter1 = 0;
    int counter2 = 0;

    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        // The GameObject cannot jump
        m_Jump = false;
    }

    void Update()
    {

        counter1++;

        if (counter1 == counter2)
        {
            m_Jump = false;
        }


        //Click the mouse or tap the screen to change the animation
        if (Input.GetKey(KeyCode.R))
        {
            m_Jump = true;
            counter2 = counter1 + 200;
        }             





            //If the GameObject is not jumping, send that the Boolean “Jump” is false to the Animator. The jump animation does not play.


        if (m_Animator != null)// animator is of type "Animator"
        {
            
            if(m_Animator.runtimeAnimatorController!=null)
            {
                if (m_Jump == false)
                m_Animator.SetBool("tommyReload", false);
                m_Animator.SetBool("MP40Reloading", false);
                m_Animator.SetBool("M1911Reload", false);
            }
        }




        //The GameObject is jumping, so send the Boolean as enabled to the Animator. The jump animation plays.
        if (m_Animator != null)// animator is of type "Animator"
        {
            
            if(m_Animator.runtimeAnimatorController!=null)
            {
                if (m_Jump == true)
                {
                m_Animator.SetBool("tommyReload", true);
                m_Animator.SetBool("MP40Reloading", true);
                m_Animator.SetBool("M1911Reload", true);                
                }
            }
        }
    }
}
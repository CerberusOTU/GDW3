using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Animator animator;
    private Weapon weapon;
    public GameObject M1911;
    public GameObject Tommy;
    public GameObject Revolver;
    public GameObject MP40;
    public GameObject Shotgun;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        weapon = GameObject.FindObjectOfType<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if (weapon.currentIndex == 1)
        {
            if(Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("Switch", 11);
                    animator.SetBool("isSprinting", true);
                    //transform.Rotate(new Vector3(0f,-20f,0f)); 
                }
                else
                {
                animator.SetInteger("Switch", 1);
                animator.SetBool("isSprinting", false);
                }
            
            M1911.SetActive(true);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
            MP40.SetActive(false);
            Shotgun.SetActive(false);
        }
        if (weapon.currentIndex == 0)
        {
            if(weapon.loadout[0].name == "Tommy")
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("Switch", 22);
                    animator.SetBool("isSprinting", true);
                }
                else
                {
                animator.SetInteger("Switch", 2);
                animator.SetBool("isSprinting", false);
                }

            M1911.SetActive(false);
            Tommy.SetActive(true);
            Revolver.SetActive(false);
            MP40.SetActive(false);
            Shotgun.SetActive(false);
            }
        
        
            if(weapon.loadout[0].name == "Revolver")
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("Switch", 33);
                    animator.SetBool("isSprinting", true);
                }
                else
                {
                animator.SetInteger("Switch", 3);
                animator.SetBool("isSprinting", false);
                }
            
            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(true);
            MP40.SetActive(false);
            Shotgun.SetActive(false);
            }

             if(weapon.loadout[0].name == "MP40")
            {
                 if(Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("Switch", 44);
                    animator.SetBool("isSprinting", true);
                }
                else
                {
                animator.SetInteger("Switch", 4);
                animator.SetBool("isSprinting", false);
                }

            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
            MP40.SetActive(true);
            Shotgun.SetActive(false);
            }

             if(weapon.loadout[0].name == "Shotgun")
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetInteger("Switch", 55);
                    animator.SetBool("isSprinting", true);
                }
                else
                {
                animator.SetInteger("Switch", 5);
                animator.SetBool("isSprinting", false);
                }
                
            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
            MP40.SetActive(false);
            Shotgun.SetActive(true);
            }


        }

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        move(x, y);

    }

    private void move(float x, float y)
    {
        animator.SetFloat("ValX", x);
        animator.SetFloat("ValY", y);
    }

}
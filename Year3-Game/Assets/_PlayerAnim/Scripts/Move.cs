using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Animator animator;
    private Motion movement;
    private Weapon weapon;
    private Movement2 movement2;
    private Weapon2 weapon2;
    private Movement3 movement3;
    private Weapon3 weapon3;
    private Movement4 movement4;
    private Weapon4 weapon4;
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
        weapon2 = GameObject.FindObjectOfType<Weapon2>();
        weapon3 = GameObject.FindObjectOfType<Weapon3>();
        weapon4 = GameObject.FindObjectOfType<Weapon4>();

        movement = GameObject.FindObjectOfType<Motion>();
        movement2 = GameObject.FindObjectOfType<Movement2>();
        movement3 = GameObject.FindObjectOfType<Movement3>();
        movement4 = GameObject.FindObjectOfType<Movement4>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        //player 1 animations
        if (this.gameObject.name == "FinalChar")
        {
            if (weapon.currentIndex == 1)
            {
                if (movement.isSprinting)
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
                if (weapon.loadout[0].name == "Tommy")
                {
                    if (movement.isSprinting)
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


                if (weapon.loadout[0].name == "Revolver")
                {
                    if (movement.isSprinting)
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

                if (weapon.loadout[0].name == "MP40")
                {
                    if (movement.isSprinting)
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

                if (weapon.loadout[0].name == "Shotgun")
                {
                    if (movement.isSprinting)
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
        }
        else if (this.gameObject.name == "FinalChar2")
        {
            if (weapon2.currentIndex == 1)
            {
                if (movement2.isSprinting)
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
            if (weapon2.currentIndex == 0)
            {
                if (weapon2.loadout[0].name == "Tommy")
                {
                    if (movement2.isSprinting)
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


                if (weapon2.loadout[0].name == "Revolver")
                {
                    if (movement2.isSprinting)
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

                if (weapon2.loadout[0].name == "MP40")
                {
                    if (movement2.isSprinting)
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

                if (weapon2.loadout[0].name == "Shotgun")
                {
                    if (movement2.isSprinting)
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
        }
        else if (this.gameObject.name == "FinalChar3")
        {
            if (weapon3.currentIndex == 1)
            {
                if (movement3.isSprinting)
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
            if (weapon3.currentIndex == 0)
            {
                if (weapon3.loadout[0].name == "Tommy")
                {
                    if (movement3.isSprinting)
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


                if (weapon3.loadout[0].name == "Revolver")
                {
                    if (movement3.isSprinting)
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

                if (weapon3.loadout[0].name == "MP40")
                {
                    if (movement3.isSprinting)
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

                if (weapon3.loadout[0].name == "Shotgun")
                {
                    if (movement3.isSprinting)
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
        }
        else if (this.gameObject.name == "FinalChar4")
        {
            if (weapon4.currentIndex == 1)
            {
                if (movement4.isSprinting)
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
            if (weapon4.currentIndex == 0)
            {
                if (weapon4.loadout[0].name == "Tommy")
                {
                    if (movement4.isSprinting)
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


                if (weapon4.loadout[0].name == "Revolver")
                {
                    if (movement4.isSprinting)
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

                if (weapon4.loadout[0].name == "MP40")
                {
                    if (movement4.isSprinting)
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

                if (weapon4.loadout[0].name == "Shotgun")
                {
                    if (movement4.isSprinting)
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
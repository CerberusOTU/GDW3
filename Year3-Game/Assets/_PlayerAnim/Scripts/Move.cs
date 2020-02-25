using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
 private Animator animator;
    private playerWeaponManager weapon;
    public GameObject M1911;
    public GameObject Tommy;
    public GameObject Revolver;
    public GameObject MP40;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        weapon = GameObject.FindObjectOfType<playerWeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if (weapon.selectedWeapon == 0)
        {
            animator.SetInteger("Switch", 1);
            M1911.SetActive(true);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
            MP40.SetActive(false);
        }
        if (weapon.selectedWeapon == 1)
        {
            if(weapon.loadout[1].name == "Tommy")
            {
            animator.SetInteger("Switch", 2);
            M1911.SetActive(false);
            Tommy.SetActive(true);
            Revolver.SetActive(false);
            MP40.SetActive(false);
            }
        
        
            if(weapon.loadout[1].name == "Revolver")
            {
            animator.SetInteger("Switch", 3);
            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(true);
            MP40.SetActive(false);
            }

            if(weapon.loadout[0].name == "MP40")
            {
            animator.SetInteger("Switch", 4);
            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
            MP40.SetActive(true);
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
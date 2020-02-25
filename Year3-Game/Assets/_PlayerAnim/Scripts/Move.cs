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
            animator.SetInteger("Switch", 1);
            M1911.SetActive(true);
            Tommy.SetActive(false);
            Revolver.SetActive(false);
        }
        if (weapon.currentIndex == 0)
        {
            if(weapon.loadout[0].name == "Tommy")
            {
            animator.SetInteger("Switch", 2);
            M1911.SetActive(false);
            Tommy.SetActive(true);
            Revolver.SetActive(false);
            }
        
        
            if(weapon.loadout[0].name == "Revolver")
            {
            animator.SetInteger("Switch", 3);
            M1911.SetActive(false);
            Tommy.SetActive(false);
            Revolver.SetActive(true);
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
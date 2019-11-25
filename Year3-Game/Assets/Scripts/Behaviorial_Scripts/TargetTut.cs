using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTut : MonoBehaviour
{
    
    public float health = 50f;

    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Die()
    {
        Destroy(gameObject);
    }
}

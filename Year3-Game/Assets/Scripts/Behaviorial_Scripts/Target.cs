using UnityEngine;

public class Target : MonoBehaviour
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

    void Die()
    {
        //Destroy(gameObject);
        this.gameObject.transform.position = new Vector3(20f,3.5f,7f);
        health = 100f;
    }
}

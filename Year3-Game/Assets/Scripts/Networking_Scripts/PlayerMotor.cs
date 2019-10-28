//Cerberus GDW Code

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //gets movement vec 
    public void Move(Vector3 _vel)
    {
        velocity = _vel;
    }

    //Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
    }

    //perform movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
   

}

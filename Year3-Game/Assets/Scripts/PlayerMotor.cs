using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 Camrotation = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        //locking the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    //gets movement vec 
    public void Move(Vector3 _vel)
    {
        velocity = _vel;
    }

    //gets rotation vec 
    public void Rotate(Vector3 _rot)
    {
        rotation = _rot;
    }

    //gets Camera rotation vec 
    public void CamRotate(Vector3 _Camrot)
    {
        Camrotation = _Camrot;
    }

    //Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //perform movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    //perform rotation based on Rotation variable
    void PerformRotation()
    {
       rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

       if(cam != null)
        {
            cam.transform.Rotate(-Camrotation);
        }
        
    }
   

}

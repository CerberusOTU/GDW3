using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;

    public Rigidbody rb;
    public CapsuleCollider col;
    //jumping var
    private float distToGround;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        distToGround = col.bounds.extents.y;
    }

    //Check if player is grounded
    bool isGrounded()
    {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update()
    {

        if (Input.GetButton("Fire3"))
        {
            speed = 15f;
        }
        else
        {
            speed = 5f;
        }

        //calculate movement velocity as 3D vec

        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        //our final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        //apply the movement

        motor.Move(velocity); 

        //calculate rotation as 3D vec: Only for character to turn around
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;

        //apply player rotation
        motor.Rotate(_rotation);

        //calculate rotation as 3D vec: Only for Camera Look Around
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _CamRotation = new Vector3(xRot, 0f, 0f) * lookSensitivity;

        //apply Camera rotation
        motor.CamRotate(_CamRotation);

        
        if (isGrounded() && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 0.8f, ForceMode.Impulse);
        } 
    }

}

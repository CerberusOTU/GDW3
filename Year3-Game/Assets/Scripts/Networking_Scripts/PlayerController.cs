//Cerberus GDW Code

using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private PlayerMotor motor;

    public Rigidbody rb;
    public Collider col;

    //jumping var
    private float distToGround;

    //crouching Var
    Vector3 origScale;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        distToGround = col.bounds.extents.y;
        origScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
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

        
        if (isGrounded() && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        } 

        if (Input.GetKey(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
        } 
        else
        {
            transform.localScale = origScale;
        }
    }

}

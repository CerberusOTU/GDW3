using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public float speed;
    public float sprintModifier;
    private Rigidbody rb;
    private CapsuleCollider col;
    private float distToGround;

    public float jumpForce;
    //public GameObject groundDetect;
    public LayerMask ground;

    //camera FOV when sprinting variables
    public Camera cam;
    private float baseFOV;
    private float FOVmod = 0.98f;

    void Start()
    {
        baseFOV = cam.fieldOfView;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        distToGround = col.bounds.extents.y;
    }
    //Check if player is grounded
    bool isGrounded()
    {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f, ground);
    }

    private void Update()
    {
         if (isGrounded() && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        } 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        //bool isGrounded = Physics.Raycast(groundDetect.transform.localPosition, Vector3.down, 0.1f, ground);
        //bool jump = Input.GetKey(KeyCode.Space);
        //bool isJumping = jump && isGrounded;

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && verticalMove > 0; //&& !isJumping;

        Vector3 direction = new Vector3(horizontalMove, 0, verticalMove);
        direction.Normalize();
        
        float adjustedSpeed = speed;

        if(isSprinting)
        {
            adjustedSpeed *= sprintModifier;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }

        Vector3 targetVelocity = transform.TransformDirection(direction) * adjustedSpeed * Time.fixedDeltaTime;
        targetVelocity.y = rb.velocity.y;
        rb.velocity =  targetVelocity;

        
    }
}

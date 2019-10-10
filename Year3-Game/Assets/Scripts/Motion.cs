using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{

    float horizontalMove;
    float verticalMove;
    public float speed;
    public float sprintModifier;

    public bool isSprinting;
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

    //HeadBob variables
    public Transform weaponParent;
    private Vector3 weaponParentOrigin;

    private float movementCounter;
    private float idleCounter;

    private Vector3 targetBob;

    /////////////////
    void Start()
    {
        baseFOV = cam.fieldOfView;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        distToGround = col.bounds.extents.y;

        weaponParentOrigin = weaponParent.localPosition;
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

        if(horizontalMove == 0 && verticalMove == 0)
        {
            if(Input.GetMouseButton(1))
            {
                //stop headbob && swaying
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentOrigin ,Time.deltaTime * 2f);
            }
            else
            {
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob,Time.deltaTime * 2f);
            }
        }
        else if(isSprinting)
        {
            HeadBob(movementCounter, 0.05f, 0.025f);
            movementCounter += Time.deltaTime * 7;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob,Time.deltaTime * 15f);
        }
        else
        {
            if(Input.GetMouseButton(1))
            {
                HeadBob(movementCounter, 0.005f, 0.005f);
                movementCounter += Time.deltaTime * 5;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob,Time.deltaTime * 3f);
            }
            else
            {
                HeadBob(movementCounter, 0.025f, 0.025f);
                movementCounter += Time.deltaTime * 5;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob,Time.deltaTime * 6f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        //bool isGrounded = Physics.Raycast(groundDetect.transform.localPosition, Vector3.down, 0.1f, ground);
        //bool jump = Input.GetKey(KeyCode.Space);
        //bool isJumping = jump && isGrounded;

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isSprinting = sprint && verticalMove > 0; //&& !isJumping;

        Vector3 direction = new Vector3(horizontalMove, 0, verticalMove);
        direction.Normalize();
        
        float adjustedSpeed = speed;

        //allow the player to sprint        
        if(isSprinting)
        {
            adjustedSpeed *= sprintModifier;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
        }
        //slow down character if walking and aiming down sights
        else if(Input.GetMouseButton(1))
        {
            adjustedSpeed = speed / 2;
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }

        Vector3 targetVelocity = transform.TransformDirection(direction) * adjustedSpeed * Time.fixedDeltaTime;
        targetVelocity.y = rb.velocity.y;
        rb.velocity =  targetVelocity;
    }

    void HeadBob(float _z, float xIntensity, float yIntensity) 
    {
        targetBob = weaponParentOrigin + new Vector3(Mathf.Cos(_z) * xIntensity, Mathf.Sin(_z * 2) * yIntensity,0);
    }
}

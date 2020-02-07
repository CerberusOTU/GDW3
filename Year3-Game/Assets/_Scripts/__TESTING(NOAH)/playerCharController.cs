using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharController : MonoBehaviour
{

    CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float speed = 15f;
    private float desiredSpeed = 0f;
    private float currentSpeed = 0f;
    [SerializeField] private float sprintModifier = 2f;
    [SerializeField] private float crouchModifier = 0.5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.1f;


    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 3f;
    private bool canJump = true;

    [Header("Ground Check")]
    private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;



    private Vector3 velocity = new Vector3(0f, 0f, 0f);
    private bool isGrounded;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        groundCheck = gameObject.transform.Find("GroundCheck");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleMovement()
    {
        //Reset Speed

        //Run GroundCheck
        GroundCheck();

        //Movement input (x,z) plane
        float x = Input.GetAxisRaw(inputConstants.c_AxisNameHorizontal);
        float z = Input.GetAxisRaw(inputConstants.c_AxisNameVertical);
        Vector3 move = transform.right * x + transform.forward * z;

        if (x != 0 || z != 0)
        {
            desiredSpeed = speed;
            currentSpeed += speed * acceleration;
        }
        else
        {
            desiredSpeed = 0f;
            currentSpeed -= speed * deceleration;
        }
        //Handle Sprint and Crouch
        if (Input.GetButton(inputConstants.c_ButtonNameSprint))
        {
            desiredSpeed *= sprintModifier;
        }
        else if (Input.GetButton(inputConstants.c_ButtonNameCrouch) && isGrounded)
        {
            desiredSpeed *= crouchModifier;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, desiredSpeed);
        Debug.Log(desiredSpeed);
        //Apply movement to character controller
        characterController.Move(move * currentSpeed * Time.fixedDeltaTime);

        //Allow Jump
        if (Input.GetButton(inputConstants.c_ButtonNameJump) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Add Gravity (y) plane
        velocity.y += gravity * Time.fixedDeltaTime;
        characterController.Move(velocity * Time.fixedDeltaTime);


    }


    void GroundCheck()
    {
        //Check if hit ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //If on ground, stop downward velocity
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundDistance;
        }
    }
}

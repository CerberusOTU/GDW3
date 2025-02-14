﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class Motion : MonoBehaviour
{
    Controller controller;

    float horizontalMove;
    float verticalMove;

    Vector3 targetVelocity;
    public float speed;
    public float sprintModifier;

    public bool isSprinting;

    private bool moving;
    private Rigidbody rb;
    private BoxCollider col;
    private float distToGround;

    private bool jumpInUse;
    public float jumpForce;
    public LayerMask ground;

    public GameObject groundDetect;
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

    private bool isCrouching = false;
    private Vector3 baseCamTrans;
    private Vector3 crouchCamTrans;
    private bool crouchLerp;
    private bool crouchInUse;

    private float gravity;
    private playerWeaponManager _weapon; // Link Weapon Script

    //**************TUTORIAL VARIABLES**************/
    [System.NonSerialized]
    private Tutorial_Manager _tutManager;

    /////////////////
    void Start()
    {
        baseFOV = cam.fieldOfView;
        rb = this.gameObject.GetComponent<Rigidbody>();
        col = this.gameObject.GetComponent<BoxCollider>();
        _weapon = gameObject.GetComponent<playerWeaponManager>();
        _tutManager = GameObject.FindObjectOfType<Tutorial_Manager>();

        distToGround = col.bounds.extents.y;

        weaponParentOrigin = weaponParent.localPosition;

        baseCamTrans = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z);
        crouchCamTrans = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y - 1f, cam.transform.localPosition.z);
        controller = GameObject.FindObjectOfType<Controller>();
    }
    //Check if player is grounded
    bool isGrounded()
    {
        return Physics.Raycast(groundDetect.transform.position, Vector3.down,0.1f);
    }

    void Crouch()
    {
        //if(Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Crouch"))
        if (controller.state.Buttons.B == ButtonState.Pressed && controller.prevState.Buttons.B == ButtonState.Released)
        {
            if (isCrouching == true)
            {
                isCrouching = false;
            }
            else
            {
                isCrouching = true;
            }
            //Tutorial completion check
            if (!_tutManager.b_crouchComplete)
                _tutManager.Notify("CROUCH_COMPLETE");
        }

        if (isCrouching == true)
        {
            //col.height = 1f;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, crouchCamTrans, Time.deltaTime * 5f);

        }
        else
        {
            //col.height = 2f;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, baseCamTrans, Time.deltaTime * 5f);

        }

    }

    void getCrouchDown()
    {
        if (controller.state.Buttons.B == ButtonState.Pressed)
        {
            if (crouchInUse == false)
            {
                // Call your event function here.
                Crouch();
                crouchInUse = true;
            }
        }
        if (controller.state.Buttons.B == ButtonState.Released)
        {
            crouchInUse = false;
        }
    }

    void Update()
    {
        if(!isGrounded())
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 2f;
        }
        
        //check crouch state
        //getCrouchDown();
        Crouch();
        getJumpDown();
        /* if (isGrounded() && (Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("Jump")))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }  */

        if (!isGrounded())
        {
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentOrigin, Time.deltaTime * 2f);
        }
        else if (horizontalMove == 0 && verticalMove == 0)
        {
            if (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
            {
                //stop headbob && swaying
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentOrigin, Time.deltaTime * 2f);
            }
            else if (isCrouching == true)
            {
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime);
            }
            else
            {
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime * 2f);
            }
        }
        else if (isSprinting)
        {
            HeadBob(movementCounter, 0.01f, 0.025f);
            movementCounter += Time.deltaTime * 7;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime * 15f);
        }
        else
        {
            if (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
            {
                if (isCrouching == true)
                {
                    HeadBob(movementCounter, 0.005f, 0.005f);
                    movementCounter += Time.deltaTime * 5;
                    weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime);
                }
                else
                {
                    HeadBob(movementCounter, 0.005f, 0.005f);
                    movementCounter += Time.deltaTime * 5;
                    weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime * 3f);
                }
            }
            else if (isCrouching == true)
            {
                HeadBob(movementCounter, 0.025f, 0.025f);
                movementCounter += Time.deltaTime * 5;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime * 3f);
            }
            else
            {
                HeadBob(movementCounter, 0.025f, 0.025f);
                movementCounter += Time.deltaTime * 5;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBob, Time.deltaTime * 6f);
            }
        }

    }

    void getJumpDown()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpInUse == false && isGrounded())
            {
                // Call your event function here.
                rb.AddForce(Vector3.up * (jumpForce * 10f), ForceMode.Impulse);
                jumpInUse = true;
            }
        }

        if (controller.state.Buttons.A == ButtonState.Pressed)
        {
            if (jumpInUse == false && isGrounded())
            {
                // Call your event function here.
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpInUse = true;
            }
        }
        if (controller.state.Buttons.A == ButtonState.Released)
        {
            jumpInUse = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (isGrounded())
        //{
            if (controller.state.IsConnected)
            {
                horizontalMove = controller.state.ThumbSticks.Left.X;
                moving = true;
            }
            else
            {
                horizontalMove = Input.GetAxisRaw("Horizontal");
                moving = true;
            }
            if (controller.state.IsConnected)
            {
                verticalMove = controller.state.ThumbSticks.Left.Y;
                moving = true;
            }
            else
            {
                verticalMove = Input.GetAxisRaw("Vertical");
                moving = true;
            }
        //}

        if(horizontalMove == 0 && verticalMove == 0)
        {
            moving = false;
        }

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || controller.state.Buttons.LeftStick == ButtonState.Pressed;
        isSprinting = sprint && verticalMove > 0; //&& !isJumping;

        Vector3 direction = new Vector3(horizontalMove, 0, verticalMove);
        direction.Normalize();

        float adjustedSpeed = speed;

        if (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1)
        {
            isSprinting = false;
            adjustedSpeed = speed;
        }

        //allow the player to sprint        
        if (isSprinting)
        {
            adjustedSpeed *= sprintModifier;
            //weaponParent.localRotation = Quaternion.Euler(0f, 270f, 0f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * FOVmod, Time.deltaTime * 8f);
        }
        //slow down character if walking and aiming down sights
        else if ((Input.GetMouseButton(1) || controller.state.Triggers.Left == 1) && isCrouching == false)
        {
            //adjustedSpeed = speed / 1.5f;
            adjustedSpeed = speed / 1.10f;
        }
        else if (isCrouching == true && (Input.GetMouseButton(1) || controller.state.Triggers.Left == 1))
        {
            //adjustedSpeed = speed / 1.85f;
            adjustedSpeed = speed / 1.20f;
        }
        else if (isCrouching == true && (!Input.GetMouseButton(1) || controller.state.Triggers.Left == 0))
        {
            //adjustedSpeed = speed / 1.75f;
            adjustedSpeed = speed / 1.15f;
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }
          
          if(moving)
          {
              Vector3 movementSide = transform.right * horizontalMove;
              Vector3 movementForward = transform.forward * verticalMove;

             if((Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.S) ) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
             {
                rb.AddForce((movementSide * adjustedSpeed) / 1.375f);
                rb.AddForce((movementForward * adjustedSpeed) / 1.375f);
             }
             else
             {
              rb.AddForce(movementSide * adjustedSpeed);
              rb.AddForce(movementForward * adjustedSpeed);
             }
           } 
           else if(!moving && isGrounded())
            {
                rb.velocity = Vector3.zero;
            }    
    }

    void HeadBob(float _z, float xIntensity, float yIntensity)
    {
        targetBob = weaponParentOrigin + new Vector3(Mathf.Cos(_z) * xIntensity, Mathf.Sin(_z * 2) * yIntensity, 0);
    }
}

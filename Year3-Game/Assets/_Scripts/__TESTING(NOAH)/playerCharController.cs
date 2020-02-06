using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharController : MonoBehaviour
{

    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GroundCheck();
    }

    public void GroundCheck()
    {
        //float distanceToGround = 0f;

        //if(Physics.CapsuleCast(capsulePoint1(), capsulePoint2(), characterController.radius, -transform.up, Mathf.Infinity, , QueryTriggerInteraction.Ignore))
        //{

        //}

    }

    //Point 1 - Center of sphere at the START of the capsule
    Vector3 capsulePoint1()
    {
        return transform.position + characterController.center + Vector3.up * -characterController.height * 0.5F;
    }
    //Point 2 - Center of sphere at the START of the capsule
    Vector3 capsulePoint2()
    {
        return capsulePoint1() + Vector3.up * characterController.height;
    }
}

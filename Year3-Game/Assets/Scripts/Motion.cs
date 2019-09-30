using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public float speed;
    public float sprintModifier;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");
        
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && verticalMove > 0;

        Vector3 direction = new Vector3(horizontalMove, 0, verticalMove);
        direction.Normalize();
        
        float adjustedSpeed = speed;

        if(isSprinting)
        {
            adjustedSpeed *= sprintModifier;
        }

        rb.velocity = transform.TransformDirection(direction) * adjustedSpeed * Time.fixedDeltaTime;
    }
}

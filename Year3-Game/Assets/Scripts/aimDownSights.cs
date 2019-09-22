using UnityEngine;

public class aimDownSights : MonoBehaviour
{
    public Vector3 aimDownPos;
    public Vector3 hitFirePos;

    public Canvas crossHair;
    float aimSpeed = 0.10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownPos, aimSpeed);
            crossHair.enabled = false;
        }
        else
        {
            transform.localPosition =  Vector3.Slerp(transform.localPosition, hitFirePos, aimSpeed);
            //transform.localPosition = hitFirePos;
            crossHair.enabled = true;
        }
    }
}

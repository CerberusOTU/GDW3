using UnityEngine;

public class aimDownSights : MonoBehaviour
{
    public Vector3 aimDownPos;
    public Vector3 hitFirePos;

    public Canvas crossHair;
    float aimSpeed = 0.15f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownPos, aimSpeed /** Time.time*/);
            crossHair.enabled = false;
        }
        else
        {
            transform.localPosition = hitFirePos;
            crossHair.enabled = true;
        }
    }
}

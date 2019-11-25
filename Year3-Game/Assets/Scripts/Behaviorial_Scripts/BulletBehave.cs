using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehave : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(2f);
        this.transform.gameObject.SetActive(false);
    }
}

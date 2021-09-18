using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineControlRemote : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        float xRotation = Mathf.Clamp(transform.root.GetComponent<PlayerManager>().spineAngle, -60f, 60f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, xRotation);
    }
}


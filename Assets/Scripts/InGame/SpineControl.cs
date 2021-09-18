using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineControl : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerCamera;
    public float xRotation = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
        xRotation = playerCamera.rotation.eulerAngles.x;
        xRotation = (xRotation > 180) ? xRotation - 360 : xRotation;
    }
}


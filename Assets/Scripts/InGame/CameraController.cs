﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
       // verticalRotation = transform.localEulerAngles.x;
        //horizontalRotation = player.transform.eulerAngles.y;
        HideCursor();
    }

    private void Update()
    {
        /*if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }*/

        //Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    public void ShowCursor()
    {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

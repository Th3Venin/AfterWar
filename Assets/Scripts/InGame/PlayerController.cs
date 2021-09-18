using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public CharacterController controller;
    public float moveSpeed = 4f;
    public float jumpSpeed = 5f;
    private float yVelocity = 0;
    public float gravity = -9.81f;
    Vector3 moveDirection;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           /* if (GameManager.players[Client.instance.gameId].remainingAmmo > 0 &&
                GameManager.players[Client.instance.gameId].health > 0)
            {
                //ClientSend.PlayerShoot(camTransform.forward);
                GameManager.players[Client.instance.gameId].Shoot();
            }*/
        }
    }
    private void FixedUpdate()
    {
        //Move();
    }

    private void Move()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            Input.GetKey(KeyCode.LeftShift)
        };

        Vector2 inputDirection = Vector2.zero;

        if (inputs[0])
        {
            inputDirection.y += 1;
        }
        if (inputs[1])
        {
            inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            inputDirection.x += 1;
        }

        moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0;

            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }

            if (inputs[5])
                moveDirection *= 2;
        }

        yVelocity += gravity;

        moveDirection.y = yVelocity;


        controller.Move(moveDirection);
       // ClientSend.PlayerMovement(transform.position, transform.rotation, camTransform.rotation);
    }

    private bool IsMoving()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D)
        };

        if (inputs[0] || inputs[0] || inputs[2] || inputs[3])
            return true;

        return false;
    }
}

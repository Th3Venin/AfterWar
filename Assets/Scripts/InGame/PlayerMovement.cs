using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 10f;
    float groundedThreshold = .5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    public bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;


    public Rigidbody rb;
    public Transform camTransform;
    protected CapsuleCollider capsule;
    public SpineControl spine;

    RaycastHit slopeHit;

    public bool[] keysPressed;

    private bool disableShooting = false;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        keysPressed = new bool[8];
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        MyInput();
        ControlDrag();
        ControlSpeed();
        CheckIfGrounded();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void CheckKeysPressed()
    {
        keysPressed[0] = Input.GetKey(KeyCode.W);
        keysPressed[1] = Input.GetKey(KeyCode.A);
        keysPressed[2] = Input.GetKey(KeyCode.S);
        keysPressed[3] = Input.GetKey(KeyCode.D);
        keysPressed[4] = Input.GetKey(KeyCode.Space);
        keysPressed[5] = Input.GetKey(KeyCode.LeftShift);
        keysPressed[6] = Input.GetKey(KeyCode.Mouse0);
        keysPressed[7] = Input.GetKey(KeyCode.Mouse1);
    }

    //runs per physics iteration
    private void FixedUpdate()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            ClientSend.PlayerMovement(this);
            return;
        }

        MovePlayer();

        if (!isGrounded) // stronger gravitational force
            rb.AddForce(Physics.gravity * rb.mass);


        CheckKeysPressed();
        ClientSend.PlayerMovement(this);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && verticalMovement > 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope()) // normal movement
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope()) // slope control
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded) // wall slide
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    protected void CheckIfGrounded()
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;

        isGrounded = false;
        float maxDistance = groundedThreshold;

        //aruncam 9 raze in jos, din centrul si de pe conturul(cercul) capsulei ca sa detectam pamant sub picioare
        for (float xOffset = -1f; xOffset <= 1f; xOffset += 1f)
        {
            for (float zOffset = -1f; zOffset <= 1f; zOffset += 1f)
            {
                Vector3 finalCapsulePos = transform.position - new Vector3(0f, capsule.height / 2f) + capsule.center;
                Vector3 capsuleContourOffset = new Vector3(xOffset, 0f, zOffset).normalized * capsule.radius;
                ray.origin = finalCapsulePos + Vector3.up * groundedThreshold / 2f + capsuleContourOffset;
                if (Physics.Raycast(ray, maxDistance))
                {//handle ray intersection, personajul e pe pamant
                    isGrounded = true;
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.green);
                }
                else
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.red);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    Animator animator;

    PlayerMovement movement;

    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        movement = this.transform.GetComponent<PlayerMovement>();
        stats = this.transform.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && stats.hasWeapon)
        {
            animator.Play("shoot");
        }

        if (movement.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetFloat("jump", 1);
                animator.Play("jump");
                StartCoroutine(WaitJump());
                animator.SetFloat("jump", 0);

            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 2, 1f, Time.deltaTime * 10f);
                animator.speed = 1f;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 1, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", -1, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("xCoord", 1, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 0, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetFloat("xCoord", -1, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 0, 1f, Time.deltaTime * 10f);
                animator.speed = 1.2f;
            }
            else
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 0, 1f, Time.deltaTime * 10f);
                animator.speed = 1f;
            }
        }
    }

    IEnumerator WaitJump()
    {
        yield return new WaitForSeconds(1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlRemote: MonoBehaviour
{
    public bool[] keysPressed;
    Animator animator;

    public PlayerManager playerManager;
    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        keysPressed = new bool[7];
        animator = this.transform.GetComponent<Animator>();
        stats = this.transform.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager.keysPressed[6] && stats.hasWeapon)
        {
            animator.Play("shoot");
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.keysPressed[4])
            {
                animator.SetFloat("jump", 1);
                animator.Play("jump");
                StartCoroutine(WaitJump());
                animator.SetFloat("jump", 0);

            }
            else if (playerManager.keysPressed[0] && playerManager.keysPressed[5])
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 2, 1f, Time.deltaTime * 10f);
                animator.speed = 1f;
            }
            else if (playerManager.keysPressed[0])
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 1, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (playerManager.keysPressed[2])
            {
                animator.SetFloat("xCoord", 0, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", -1, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (playerManager.keysPressed[3])
            {
                animator.SetFloat("xCoord", 1, 1f, Time.deltaTime * 10f);
                animator.SetFloat("yCoord", 0, 1f, Time.deltaTime * 10f);
                animator.speed = 1.4f;
            }
            else if (playerManager.keysPressed[1])
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

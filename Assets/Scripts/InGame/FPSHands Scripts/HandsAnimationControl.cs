using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAnimationControl : MonoBehaviour
{
    public GameObject scope;

    public AudioClip clip;
    AudioSource audio;

    Animator animator;

    PlayerMovement movement;

    Transform camera;

    bool isReloading = false;

    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        movement = this.transform.root.GetComponent<PlayerMovement>();
        camera = RecursiveFindChild(this.transform.root, "MainCamera");
        stats = transform.root.GetComponent<PlayerStats>();

        InstantiateAudio(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.isReloading)
        {
            // Debug.Log("Not reloading");

            if (stats.equippedWeapon == WeaponTypes.NoWeapon) // no weapon animations
            {
                animator.SetInteger("treeSelector", 1);

                if (movement.isGrounded)
                {
                    //Debug.Log("Playing animation");
                    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) //forward run
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 2, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) //walk
                        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    }
                    else
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f); //idle
                        animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    }
                }
            }


            if (stats.currentWeapon == null)
            {
                //Debug.Log("NULL WEAPON");
                return;
            }

            int magazineSize = stats.currentWeapon.GetComponent<Weapon>().magazineSize;
            bool magEmpty = stats.magazine == 0 && stats.reserve != 0;
            bool reload = Input.GetKeyDown(KeyCode.R) && magazineSize != stats.magazine && stats.reserve != 0;

            if (stats.currentWeapon.GetComponent<AutomaticWeapon>() != null) //rifle animations
            {
                animator.SetInteger("treeSelector", 1);

                if (movement.isGrounded)
                {
                    if (reload || magEmpty) //reload
                    {
                        ReleaseScope();
                        StartCoroutine(ReloadRifle());
                        animator.Play("RifleReload");
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && stats.magazine > 0) //aim fire
                    {
                        Scope();
                        animator.SetFloat("xCoordRifle", 1, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && stats.magazine > 0) //fire
                    {
                        animator.SetFloat("xCoordRifle", 0.5f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 0.5f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse1)) //aim pose
                    {
                        Scope();
                        animator.SetFloat("xCoordRifle", 0.8f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 0.8f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse0)) //release aim
                    {
                        ReleaseScope();
                    }
                    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) // walk forward
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 2, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) //walk
                        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    }
                    else if (isReloading == false)
                    {
                        animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f); //idle
                        animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    }
                }
                else
                {
                    if (reload || magEmpty) //reload
                    {
                        ReleaseScope();
                        StartCoroutine(ReloadRifle());
                        animator.Play("RifleReload");
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && stats.magazine > 0) //aim fire
                    {
                        Scope();
                        animator.SetFloat("xCoordRifle", 1, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && stats.magazine > 0) //fire
                    {
                        animator.SetFloat("xCoordRifle", 0.5f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 0.5f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse1)) //aim pose
                    {
                        Scope();
                        animator.SetFloat("xCoordRifle", 0.8f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordRifle", 0.8f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse0)) //release aim
                    {
                        ReleaseScope();
                    }
                }
            }

            if (stats.currentWeapon.GetComponent<SingleShotWeapon>() != null) //pistol animations
            {
                animator.SetInteger("treeSelector", 2);
                Debug.Log("Single shot weapon anim");
                if (movement.isGrounded)
                {
                    if (reload || magEmpty) //reload
                    {
                        ReleaseScope();
                        StartCoroutine(ReloadPistol());
                        animator.Play("PistolReload");
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && stats.magazine > 0) //aim fire
                    {
                        Scope();
                        animator.SetFloat("xCoordPistol", 1, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 1, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && stats.magazine > 0) //fire
                    {
                        animator.SetFloat("xCoordPistol", 0.5f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 0.5f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse1)) //aim pose
                    {
                        Scope();
                        animator.SetFloat("xCoordPistol", 0.8f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 0.8f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse0)) //release aim
                    {
                        ReleaseScope();
                    }
                    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) // walk forward
                    {
                        animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 2, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) //walk
                        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    {
                        animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 1, 1f, Time.deltaTime * 10f);
                    }
                    else
                    {
                        animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f); //idle
                        animator.SetFloat("yCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    }
                }
                else
                {
                    if (reload || magEmpty) //reload
                    {
                        ReleaseScope();
                        StartCoroutine(ReloadPistol());
                        animator.Play("PistolReload");
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && stats.magazine > 0) //aim fire
                    {
                        Scope();
                        animator.SetFloat("xCoordPistol", 1, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 1, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse0) && stats.magazine > 0) //fire
                    {
                        animator.SetFloat("xCoordPistol", 0.5f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 0.5f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKey(KeyCode.Mouse1)) //aim pose
                    {
                        Scope();
                        animator.SetFloat("xCoordPistol", 0.8f, 1f, Time.deltaTime * 10f);
                        animator.SetFloat("yCoordPistol", 0.8f, 1f, Time.deltaTime * 10f);
                    }
                    else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse0)) //release aim
                    {
                        ReleaseScope();
                    }
                }
            }
        }
    }

    public Transform RecursiveFindChild(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == tag)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, tag);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    public void Scope()
    {
        HUDManager.instance.scope.SetActive(true);
        camera.GetComponent<Camera>().fieldOfView = 30;
        camera.transform.localPosition = new Vector3(0, -1f, 0);
    }

    public void ReleaseScope()
    {
        HUDManager.instance.scope.SetActive(false);
        camera.GetComponent<Camera>().fieldOfView = 60;
        camera.transform.localPosition = new Vector3(0, 0f, 0);
    }

    IEnumerator ReloadRifle()
    {
        PlaySound();
        stats.isReloading = true;
        yield return new WaitForSeconds(2.2f);
        stats.isReloading = false;
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator ReloadPistol()
    {
        PlaySound();
        stats.isReloading = true;
        yield return new WaitForSeconds(2.2f);
        stats.isReloading = false;
        GetComponent<AudioSource>().Stop();
    }

    public void InstantiateAudio(AudioClip clip)
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = clip;
    }

    public void PlaySound()
    {
        audio.playOnAwake = false;
        audio.time = 0.3f;
        if (audio.isPlaying)
            audio.Stop();
        audio.Play();
    }
}

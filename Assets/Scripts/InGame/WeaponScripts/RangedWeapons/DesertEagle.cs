using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertEagle : SingleShotWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        this.damage = 3f;
        this.range = 100f;
        this.magazineSize = 7;
        fpsCam = RecursiveFindChild(transform.root, "MainCamera");
        animator = Utils.RecursiveFindChild(transform.root, "Hands").GetComponent<Animator>();

        InstantiateAudio(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
}

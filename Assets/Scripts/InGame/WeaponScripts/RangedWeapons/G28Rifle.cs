using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G28Rifle : AutomaticWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        this.damage = 2f;
        this.range = 150f;
        this.fireRate = 3f;
        this.nextTimeToFire = 0f;
        this.magazineSize = 10;
        fpsCam = RecursiveFindChild(transform.root, "MainCamera");
        animator = Utils.RecursiveFindChild(transform.root, "Hands").GetComponent<Animator>();

        InstantiateAudio(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
}

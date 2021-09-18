using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : AutomaticWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        this.damage = 3f;
        this.range = 100f;
        this.fireRate = 10f;
        this.nextTimeToFire = 0f;
        this.magazineSize = 30;
        fpsCam = RecursiveFindChild(transform.root, "MainCamera");

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsCollector : MonoBehaviour
{
    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = this.transform.root.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Transform weaponHolder = RecursiveFindChild(this.transform.root, "WeaponHolder");

            if (weaponHolder.transform.childCount > 0)
                other.transform.gameObject.SetActive(false);

            other.transform.parent = weaponHolder;
            other.transform.localPosition = new Vector3(0.002f, 0.22f, -0.081f);
            other.transform.localRotation = Quaternion.Euler(-84, -241, 240);
            other.transform.GetComponent<MeshCollider>().enabled = false;
            other.transform.GetComponent<Weapon>().enabled = true;

            stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");
        }

        if (other.tag == "AK47")
        {
            Transform weaponHolder = RecursiveFindChild(this.transform.root, "WeaponHolder");

            if (weaponHolder.transform.childCount > 0)
                other.transform.gameObject.SetActive(false);

            other.transform.parent = weaponHolder;
            other.transform.localPosition = new Vector3(0.012f, 0.274f, 0.186f);
            other.transform.localRotation = Quaternion.Euler(5.6f, 88.5f, -93f);
            other.transform.GetComponent<MeshCollider>().enabled = false;
            other.transform.GetComponent<Weapon>().enabled = true;

            stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");
        }

        if (other.tag == "M4")
        {
            Transform weaponHolder = RecursiveFindChild(this.transform.root, "WeaponHolder");

            if (weaponHolder.transform.childCount > 0)
                other.transform.gameObject.SetActive(false);

            other.transform.parent = weaponHolder;
            other.transform.localPosition = new Vector3(0.003f, 0.1f, -0.027f);
            other.transform.localRotation = Quaternion.Euler(176f, -90f, 88.5f);
            other.transform.GetComponent<MeshCollider>().enabled = false;
            other.transform.GetComponent<Weapon>().enabled = true;

            stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");
        }
    }

    Transform RecursiveFindChild(Transform parent, string tag)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
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

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Armor" && !stats.hasArmor)
        {
            other.transform.parent = this.transform.root;
            other.transform.localPosition = new Vector3(0, 0, 0);
            other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            other.transform.GetComponent<BoxCollider>().enabled = false;

            Armor collectedArmor = other.transform.GetComponent<Armor>();

            stats.hasArmor = true;
            stats.armor = collectedArmor.armor;

            Debug.Log("Player has collected " + collectedArmor.name + "!");
            Debug.Log("Player's armor is " + stats.armor + "!");
        }

        if (other.tag == "Weapon" && !stats.hasWeapon)
        {
            Transform righthand = RecursiveFindChild(this.transform.root, "RightHand");

            other.transform.parent = righthand;
            other.transform.localPosition = new Vector3(0.156f, 0.34f, 0.036f);
            other.transform.localRotation = Quaternion.Euler(0, 180, -90);
            other.transform.GetComponent<MeshCollider>().enabled = false;
            other.transform.GetComponent<Weapon>().enabled = true;

            stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");
        }
    }*/

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

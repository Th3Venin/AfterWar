using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;

    public PlayerStats stats;
    public MeshRenderer model;

    public float spineAngle;

    public bool[] keysPressed;
    public bool isGrounded;

    public void Initialize(int id, string username, int deaths, int kills, int score)
    {
        this.id = id;
        this.username = username;
        stats.deaths = deaths;
        stats.kills = kills;
        stats.score = score;
        keysPressed = new bool[8];
    }

    public void SetKeysPressed(bool[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keysPressed[i] = keys[i];
        }
    }

    public void SetHealth(float health)
    {
        stats.health = health;
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Shoot()
    {
        // if (GameManager.instance.matchStage == MatchStage.match)
            //remainingAmmo = remainingAmmo < 1 ? maxAmmo : remainingAmmo - 1;
    }

    public void EquipWeapon(WeaponTypes type)
    {
        if (type == WeaponTypes.NoWeapon)
        {
            UnEquipWeapon();
            return;
        }

        Debug.Log("Weapon to equip " + (int)type);
        GameObject prefab = GameManager.instance.weaponPrefabs[(int)type];
        GameObject item = Instantiate(prefab, new Vector3(0.156f, 0.34f, 0.036f), Quaternion.Euler(0, 180, -90));

        Transform holder = GetWeaponHolder();

        UnEquipWeapon();

        item.transform.parent = holder;
        item.GetComponent<Weapon>().enabled = true;

        transform.root.GetComponent<PlayerStats>().equippedWeapon = type;

        if (id == Client.instance.gameId)
        {
            item.transform.localPosition = GameManager.instance.weaponPosLocal[(int)type];
            item.transform.localRotation = GameManager.instance.weaponRotLocal[(int)type];
        }
        else
        {
            item.transform.localPosition = GameManager.instance.weaponPosRemote[(int)type];
            item.transform.localRotation = GameManager.instance.weaponRotRemote[(int)type];
        }

        stats.currentWeapon = item;
        Debug.Log("Player has collected a weapon!");
    }

    public void UnEquipWeapon()
    {
        Transform holder = GetWeaponHolder();

        foreach (Transform child in holder.transform)
        {
            if (child.tag == "Weapon")
                GameObject.Destroy(child.gameObject);
        }

        transform.root.GetComponent<PlayerStats>().equippedWeapon = WeaponTypes.NoWeapon;
        stats.reserve = 0;
        stats.magazine = 0;
    }

    private Transform GetWeaponHolder()
    {
        if (id == Client.instance.gameId)
            return RecursiveFindChild(transform.root, "WeaponHolder");

        return RecursiveFindChild(transform.root, "RightHand");
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

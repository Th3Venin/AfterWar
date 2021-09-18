using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public int spawnerId;
    public bool hasItem;
    public GameObject itemModel;
    public WeaponTypes type;

    private Vector3 position;

    public float itemRotationSpeed = 50f;
    public float itemBobSpeed = 2f;


    private void Update()
    {
        if (hasItem)
        {
            transform.Rotate(Vector3.up, itemRotationSpeed * Time.deltaTime, Space.World);
            transform.position = position + new Vector3(0f, 0.25f * Mathf.Sin(Time.time * itemBobSpeed), 0);
        }
    }

    public void Intitialize(int spawnerId, bool hasItem, Vector3 position, WeaponTypes type)
    {
        this.spawnerId = spawnerId;
        itemModel.SetActive(hasItem);
        this.hasItem = hasItem;
        this.position = position;
        this.type = type;
    }

    public void ItemSpawned()
    {
        hasItem = true;
        itemModel.SetActive(true);
    }

    public void ItemPickedUp()
    {
        hasItem = false;
        itemModel.SetActive(false);
    }
}

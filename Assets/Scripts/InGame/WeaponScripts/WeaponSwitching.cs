using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public GameObject flashLight;

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            WeaponTypes type = transform.root.GetComponent<PlayerStats>().equippedWeapon;

            if (type == WeaponTypes.NoWeapon)
                return;

            ClientSend.WeaponDropped(type);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ClientSend.Reload();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon >= 3)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = 3;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            HUDManager.instance.selectedWeapon = selectedWeapon;
            ClientSend.EquipWeapon(selectedWeapon);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //flashLight.SetActive(!flashLight.activeSelf);
        }
    }
}
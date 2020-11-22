using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour 
{
    [SerializeField]
    private WeaponHandler[] weapons;
    private int currentWeaponIndex;
    private bool weaponSwapCooldown = false;

    //UI icons to display on weapon change
    public GameObject pistolUI;
    public GameObject shotgunUI;
    public GameObject meleeUI;

    void Start () 
    {
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
        meleeUI.SetActive(true);
    }
    
    void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !weaponSwapCooldown) {
            TurnOnSelectedWeapon(0);
            meleeUI.SetActive(true);
            pistolUI.SetActive(false);
            shotgunUI.SetActive(false);
            Invoke("ResetCooldown", 0.8f);
            weaponSwapCooldown = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !weaponSwapCooldown) {
            TurnOnSelectedWeapon(1);
            pistolUI.SetActive(true);
            shotgunUI.SetActive(false);
            meleeUI.SetActive(false);
            Invoke("ResetCooldown", 0.8f);
            weaponSwapCooldown = true;
        }
    
        if (Input.GetKeyDown(KeyCode.Alpha3) && !weaponSwapCooldown) {
            TurnOnSelectedWeapon(2);
            shotgunUI.SetActive(true);
            pistolUI.SetActive(false);
            meleeUI.SetActive(false);
            Invoke("ResetCooldown", 0.8f);
            weaponSwapCooldown = true;
        }
    }

    void TurnOnSelectedWeapon(int weaponIndex) 
    {

        if (currentWeaponIndex == weaponIndex)
            return;

        // turn of the current weapon
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        // turn on the selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);

        // store the current selected weapon index
        currentWeaponIndex = weaponIndex;
    }

    void ResetCooldown()
    {
        weaponSwapCooldown = false;
    }

    public WeaponHandler GetCurrentSelectedWeapon() 
    {
        return weapons[currentWeaponIndex];
    }
}


































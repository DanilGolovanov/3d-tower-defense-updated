using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour 
{
    [SerializeField]
    private WeaponHandler[] weapons;
    private int current_Weapon_Index;
    //UI icons to display on weapon change
    public GameObject pistolUI;
    public GameObject shotgunUI;
    public GameObject meleeUI;

    void Start () 
    {
        current_Weapon_Index = 0;
        weapons[current_Weapon_Index].gameObject.SetActive(true);
        meleeUI.SetActive(true);
    }
    
    void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            TurnOnSelectedWeapon(0);
            meleeUI.SetActive(true);
            pistolUI.SetActive(false);
            shotgunUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TurnOnSelectedWeapon(1);
            pistolUI.SetActive(true);
            shotgunUI.SetActive(false);
            meleeUI.SetActive(false);
        }
    
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            TurnOnSelectedWeapon(2);
            shotgunUI.SetActive(true);
            pistolUI.SetActive(false);
            meleeUI.SetActive(false);
        }
    }

    void TurnOnSelectedWeapon(int weaponIndex) 
    {

        if (current_Weapon_Index == weaponIndex)
            return;

        // turn of the current weapon
        weapons[current_Weapon_Index].gameObject.SetActive(false);

        // turn on the selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);

        // store the current selected weapon index
        current_Weapon_Index = weaponIndex;
    }

    public WeaponHandler GetCurrentSelectedWeapon() 
    {
        return weapons[current_Weapon_Index];
    }
}


































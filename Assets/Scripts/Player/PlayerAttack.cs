using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
    }

    void WeaponShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // handle melee
            if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.MELEE_TAG)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
            }
            // handle shoot
            if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                weaponManager.GetCurrentSelectedWeapon().RemoveAmmo();
                weaponManager.GetCurrentSelectedWeapon().BulletFired();
            }
            if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BUCKSHOT)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                weaponManager.GetCurrentSelectedWeapon().RemoveBuckshot();
                weaponManager.GetCurrentSelectedWeapon().BulletFired();
            }
        }

    }
}



































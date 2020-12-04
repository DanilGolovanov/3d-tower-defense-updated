using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;

public class PlayerAttack : MonoBehaviour
{
    
    public static bool reloadCheck;
    public float pistolDamage = 40f;
    public float buckshotDamage = 80f;
    private bool attackCooldown = false;
    public GameObject bloodsplatter;
    public GameObject towerMenu;
    public GameObject upgradeMenu;
    public GameObject baseMenu;
    private Camera mainCam;
    private WeaponManager weaponManager;


    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
    }

    void WeaponShoot()
    {
        if (Input.GetMouseButtonDown(0) && !attackCooldown && !reloadCheck &&!MenuCheck())
        {
            // handle melee
            if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.MELEE_TAG)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                Invoke("ResetAttackCooldown", 0.7f);
                attackCooldown = true;
            }
            // handle shoot
            if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET && !WeaponHandler.outOfAmmo)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                weaponManager.GetCurrentSelectedWeapon().RemoveAmmo();
                BulletFired();
                Invoke("ResetAttackCooldown", 0.7f);
                attackCooldown = true;
            }
            if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BUCKSHOT && !WeaponHandler.outOfBuckshot)
            {
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                weaponManager.GetCurrentSelectedWeapon().RemoveBuckshot();
                BuckShotFired();
                Invoke("ResetAttackCooldown", 0.7f);
                attackCooldown = true;
            }
        }
    }
    void ResetAttackCooldown()
    {
        attackCooldown = false;
    }
    public void BulletFired()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<Enemy>().Damage(pistolDamage);
                Instantiate(bloodsplatter, hit.transform.position, Quaternion.identity);
            }
            else
            {
                return;
            }
        }
    }
    public void BuckShotFired()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<Enemy>().Damage(buckshotDamage);
                Instantiate(bloodsplatter, hit.transform.position, Quaternion.identity);
            }
            else
            {
                return;
            }
        }
    }
    public bool MenuCheck()
    {
        if (towerMenu.activeInHierarchy || baseMenu.activeInHierarchy || upgradeMenu.activeInHierarchy)
        {
            return true;
        }
        return false;
    }
}



































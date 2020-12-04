using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;

//script to handle player attacks
public class PlayerAttack : MonoBehaviour
{
    [Header ("Player attack calibration")]
    public static bool reloadCheck;
    public float pistolDamage = 40f;
    public float buckshotDamage = 80f;
    private bool attackCooldown = false;
    //references
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

    //check what type of weapon is doing attack
    //check for cooldown, reload or in a menu
    //play animation, call melee/projectile method
    //set cooldown
    void WeaponShoot()
    {
        if (Input.GetMouseButtonDown(0) && !attackCooldown && !reloadCheck && !MenuCheck())
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
    //raycast from main camera
    //detect a hit
    //pass through damage to whatever was hit
    //damage based on ammo type
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
    //checking if game menus are active
    public bool MenuCheck()
    {
        if (towerMenu.activeInHierarchy || baseMenu.activeInHierarchy || upgradeMenu.activeInHierarchy || PauseMenu.GameIsPaused)
        {
            return true;
        }
        return false;
    }
}



































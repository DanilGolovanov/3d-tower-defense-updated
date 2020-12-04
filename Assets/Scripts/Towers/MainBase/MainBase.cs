using System.Collections;
using System.Collections.Generic;
using TowerDefence;
using UnityEngine;
using UnityEngine.UI;

//script to handle the main base and main base menu UI

public class MainBase : MonoBehaviour
{
    //ui references
    public GameObject mainBaseMenu;
    public Text notifciationText;
    public Text baseHitPoints;
    public Text currentHP;
    public Text pistolAmmo;
    public Text shotgunAmmo;

    private void Update()
    {
        baseHitPoints.text = "Base Hitpoints = " + GameManager.instance.currentbaseHitPoints.ToString();
        currentHP.text = "Health Points = " + GameManager.instance.currentHealth.ToString();
        pistolAmmo.text = "Pistol Ammo = " + WeaponHandler.maxAmmo.ToString();
        shotgunAmmo.text = "Shotgun Ammo = " + WeaponHandler.maxBuckshot.ToString();
    }
    //method to pass through damage dealt to main base
    public void TakeDamage(float damage)
    {
        if (GameManager.instance.currentbaseHitPoints < 0)
        {
            //game over
            return; //for now
        }
        GameManager.instance.currentbaseHitPoints -= damage;
    }
    //collision detection to open menus
    private void OnTriggerEnter(Collider other)
    {
        notifciationText.text = "You have $ " + Player.instance.money.ToString() + " Cash";
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.T) && !mainBaseMenu.gameObject.activeSelf)
        {
            mainBaseMenu.gameObject.SetActive(true);
            Cursor.visible = (true);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.T) && mainBaseMenu.gameObject.activeSelf)
        {
            mainBaseMenu.gameObject.SetActive(false);
            Cursor.visible = (false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    //and close menus
    private void OnTriggerExit(Collider other)
    {
        mainBaseMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    //methods for onclick functions of UI buttons on base
    public void RepairBase()
    {
        if (GameManager.instance.currentbaseHitPoints < 100)
        {
            if (Player.instance.money > 49)
            {
                Player.instance.RemoveMoney(50);
                GameManager.instance.currentbaseHitPoints = 100;
            }
            else
            {
                notifciationText.text = "You do not have enough money!";
            }
        }
        else
        {
            notifciationText.text = "Base at maximum hit points!";
        }
    }
    public void HealPlayer()
    {
        if (GameManager.instance.currentHealth < 100)
        {
            if (Player.instance.money > 49)
            {
                Player.instance.RemoveMoney(50);
                GameManager.instance.currentHealth = 100;
            }
            else
            {
                notifciationText.text = "You do not have enough money!";
            }
        }
        else
        {
            notifciationText.text = "You are at full health!";
        }
    }
    public void ReloadPistol()
    {
        if (Player.instance.money > 24)
        {
            Player.instance.RemoveMoney(25);
            WeaponHandler.maxAmmo += 12;
            notifciationText.text = "Pistol Ammo purchased";
        }
        else
        {
            notifciationText.text = "You do not have enough money!";
        }
    }
    public void ReloadShotgun()
    {
        if (Player.instance.money > 39)
        {
            Player.instance.RemoveMoney(40);
            WeaponHandler.maxBuckshot += 6;
            notifciationText.text = "Shotgun Ammo purchased";
        }
        else
        {
            notifciationText.text = "You do not have enough money!";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TowerDefence.Manager;
using TowerDefence.Towers;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    [SerializeField]
    private TowerManager towerManager;

    [SerializeField]
    private GameObject towerBuyMenu;
    [SerializeField]
    private GameObject towerUpgradeMenu;

    private TowerPlatform currentTowerPlatform;
    private bool towerExists;
    private TowerCheck currentTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PurchaseTowerOne()
    {
        TowerManager.instance.PurchaseTower(currentTowerPlatform);
        towerExists = true;
        towerBuyMenu.gameObject.SetActive(false);
        towerUpgradeMenu.gameObject.SetActive(true);
    }

    public void RemoveTower()
    {
        Destroy(currentTower.gameObject);
        towerExists = false;
        towerBuyMenu.gameObject.SetActive(true);
        towerUpgradeMenu.gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        currentTowerPlatform = other.GetComponentInParent<TowerPlatform>();
        towerExists = other.GetComponentInChildren<TowerCheck>();
        currentTower = other.GetComponentInChildren<TowerCheck>();
    }

    private void OnTriggerStay(Collider other)
    {
        currentTower = other.GetComponentInChildren<TowerCheck>();
        if (other.gameObject.tag == "TowerPlatform")
        {
            if (!towerExists)
            {
                if (Input.GetKeyDown(KeyCode.T) && !towerBuyMenu.gameObject.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    towerBuyMenu.gameObject.SetActive(true);
                }
                else if (Input.GetKeyDown(KeyCode.T) && towerBuyMenu.gameObject.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    towerBuyMenu.gameObject.SetActive(false);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.T) && !towerUpgradeMenu.gameObject.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    towerUpgradeMenu.gameObject.SetActive(true);
                }
                else if (Input.GetKeyDown(KeyCode.T) && towerUpgradeMenu.gameObject.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    towerUpgradeMenu.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TowerPlatform")
        {
            Cursor.lockState = CursorLockMode.Locked;
            towerBuyMenu.gameObject.SetActive(false);
            towerUpgradeMenu.gameObject.SetActive(false);
        }
    }
}

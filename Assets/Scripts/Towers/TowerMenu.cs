using System.Collections;
using System.Collections.Generic;
using TowerDefence;
using TowerDefence.Manager;
using TowerDefence.Towers;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text upgradePriceText;

    public void PurchaseTower(string towerTypeName)
    {
        TowerTypes towerType = RecognizeTowerType(towerTypeName);
        TowerManager.instance.PurchaseTower(currentTowerPlatform, towerType);
        towerExists = true;
        towerBuyMenu.gameObject.SetActive(false);
        towerUpgradeMenu.gameObject.SetActive(true);
        currentTower = currentTowerPlatform.GetComponentInChildren<TowerCheck>();
        currentTower.GetComponent<TowerType>().upgradeCost = currentTower.GetComponent<TowerType>().initialUpgradeCost;
        upgradePriceText.text = "$" + currentTower.GetComponentInChildren<TowerType>().upgradeCost;
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
        Player.instance.money -= currentTower.GetComponent<TowerType>().upgradeCost;

        // save tower type and all stats related to the tower type
        TowerType towerStats = currentTower.GetComponent<TowerType>();
        TowerTypes towerType = towerStats.towerType;
        float maxHealth = towerStats.maxHealth;
        float damageToGive = towerStats.damageToGive;
        float fireRange = towerStats.fireRange;
        float rechargeTime = towerStats.rechargeTime;
        bool canHitMultipleEnemies = towerStats.canHitMultipleEnemies;
        // save upgraded tower level
        int level = currentTower.GetComponent<TowerType>().level + 1;

        // destroy existing tower to build an upgraded one
        Destroy(currentTower.gameObject);

        // create empty gameObject which will hold all parts of the tower (one/multiple bases and top of the tower)
        GameObject towerHolder = new GameObject("Tower");
        towerHolder.AddComponent<TowerCheck>();
        // make the empty gameObject child of the current platform
        towerHolder.transform.SetParent(currentTowerPlatform.GetComponent<Transform>().Find("Tower Holder"));
        // set up position of the empty gameObject (just in case)
        towerHolder.transform.localPosition = Vector3.zero;

        float towerTopHeight = 0;
        // build required number of base levels
        for (int i = 0; i < level - 1; i++)
        {
            Transform towerBase = Instantiate(GetTowerBase(towerType));
            towerBase.SetParent(towerHolder.transform);
            towerTopHeight = towerBase.GetComponent<Collider>().bounds.size.y / 4 + towerBase.GetComponent<Collider>().bounds.size.y * i;
            towerBase.localPosition = new Vector3(0, towerTopHeight, 0);
        }

        // build the tower top       
        Transform towerTop = Instantiate(GetTowerTop(towerType));
        // Y position of the tower top 
        towerTopHeight += towerTop.GetComponent<Collider>().bounds.size.y;
        towerTop.SetParent(towerHolder.transform);
        towerTop.localPosition = new Vector3(0, towerTopHeight, 0);

        // assign current tower and upgrade it to a new level
        currentTower = towerHolder.GetComponent<TowerCheck>();
        // add tower type script to the tower and update its stats
        towerHolder.AddComponent<TowerType>();
        currentTower.GetComponent<TowerType>().level = level;
        currentTower.GetComponent<TowerType>().towerType = towerType;
        currentTower.GetComponent<TowerType>().maxHealth = maxHealth * currentTower.GetComponent<TowerType>().positiveStatMultiplier;
        currentTower.GetComponent<TowerType>().damageToGive = damageToGive * currentTower.GetComponent<TowerType>().positiveStatMultiplier;
        currentTower.GetComponent<TowerType>().fireRange = fireRange * currentTower.GetComponent<TowerType>().positiveStatMultiplier;
        currentTower.GetComponent<TowerType>().rechargeTime = rechargeTime * currentTower.GetComponent<TowerType>().negativeStatMultiplier;
        currentTower.GetComponent<TowerType>().canHitMultipleEnemies = canHitMultipleEnemies;
        currentTower.GetComponent<TowerType>().upgradeCost = level * currentTower.GetComponent<TowerType>().initialUpgradeCost;
        upgradePriceText.text = "$" + currentTower.GetComponentInChildren<TowerType>().upgradeCost;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentTowerPlatform = other.GetComponentInParent<TowerPlatform>();
        towerExists = other.GetComponentInChildren<TowerCheck>();
        currentTower = other.GetComponentInChildren<TowerCheck>();
        if (other.GetComponentInChildren<TowerType>())
        {
            upgradePriceText.text = "$" + other.GetComponentInChildren<TowerType>().upgradeCost;
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        currentTower = other.GetComponentInChildren<TowerCheck>();
        if (other.GetComponentInChildren<TowerType>())
        {
            upgradePriceText.text = "$" + other.GetComponentInChildren<TowerType>().upgradeCost;
        }       

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

    private Transform GetTowerBase(TowerTypes towerType)
    {
        Transform towerBase;
        switch (towerType)
        {
            case TowerTypes.DefaultTower:
                towerBase = GameAssets.GetInstance().defaultTowerBase;
                break;
            case TowerTypes.MagicTower:
                towerBase = GameAssets.GetInstance().magicTowerBase;
                break;
            case TowerTypes.SniperTower:
                towerBase = GameAssets.GetInstance().sniperTowerBase;
                break;
            case TowerTypes.MachineGunTower:
                towerBase = GameAssets.GetInstance().machineGunTowerBase;
                break;
            default:
                towerBase = GameAssets.GetInstance().defaultTowerBase;
                break;
        }
        return towerBase;
    }

    private Transform GetTowerTop(TowerTypes towerType)
    {
        Transform towerBase;
        switch (towerType)
        {
            case TowerTypes.DefaultTower:
                towerBase = GameAssets.GetInstance().defaultTowerTop;
                break;
            case TowerTypes.MagicTower:
                towerBase = GameAssets.GetInstance().magicTowerTop;
                break;
            case TowerTypes.SniperTower:
                towerBase = GameAssets.GetInstance().sniperTowerTop;
                break;
            case TowerTypes.MachineGunTower:
                towerBase = GameAssets.GetInstance().machineGunTowerTop;
                break;
            default:
                towerBase = GameAssets.GetInstance().defaultTowerTop;
                break;
        }
        return towerBase;
    }

    private TowerTypes RecognizeTowerType(string towerTypeName)
    {
        TowerTypes towerType;
        switch (towerTypeName)
        {
            case "DefaultTower":
                towerType = TowerTypes.DefaultTower;
                break;
            case "MagicTower":
                towerType = TowerTypes.MagicTower;
                break;
            case "SniperTower":
                towerType = TowerTypes.SniperTower;
                break;
            case "MachineGunTower":
                towerType = TowerTypes.MachineGunTower;
                break;
            default:
                towerType = TowerTypes.DefaultTower;
                break;
        }
        return towerType;
    }
}

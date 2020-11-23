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

    private DefaultTower defaultTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public void PurchaseTower()
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
        // save upgraded tower level
        int level = currentTower.GetComponent<DefaultTower>().level + 1;
        // destroy existing tower to build an upgraded one
        Destroy(currentTower.gameObject);

        // create empty gameObject which will hold all parts of the tower (one/multiple bases and top of the tower)
        GameObject towerHolder = new GameObject("Tower");
        towerHolder.AddComponent<TowerCheck>();
        towerHolder.AddComponent<DefaultTower>();
        // make the empty gameObject child of the current platform
        towerHolder.transform.SetParent(currentTowerPlatform.GetComponent<Transform>().Find("Tower Holder"));
        // set up position of the empty gameObject (just in case)
        towerHolder.transform.localPosition = Vector3.zero;

        float towerTopHeight = 0;
        // build required number of base levels
        for (int i = 0; i < level - 1; i++)
        {
            Transform towerBase = Instantiate(GameAssets.GetInstance().towerBase);
            towerBase.SetParent(towerHolder.transform);
            towerTopHeight = towerBase.GetComponent<Collider>().bounds.size.y / 2 + towerBase.GetComponent<Collider>().bounds.size.y * i;
            towerBase.localPosition = new Vector3(0, towerTopHeight, 0);
        }

        // build the tower top       
        Transform towerTop = Instantiate(GameAssets.GetInstance().towerTop);
        // Y position of the tower top 
        towerTopHeight += towerTop.GetComponent<Collider>().bounds.size.y;
        towerTop.SetParent(towerHolder.transform);
        towerTop.localPosition = new Vector3(0, towerTopHeight, 0);

        // assign current tower and upgrade it to a new level
        currentTower = towerHolder.GetComponent<TowerCheck>();
        currentTower.GetComponent<DefaultTower>().level = level;
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

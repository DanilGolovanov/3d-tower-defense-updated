using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Towers;
using UnityEngine.UI;

namespace TowerDefence.Manager
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance = null;

        [SerializeField]
        private List<Tower> spawnableTowers = new List<Tower>();

        [SerializeField]
        private List<Text> towerPriceTexts = new List<Text>();

        private List<Tower> aliveTowers = new List<Tower>();

        private Dictionary<TowerTypes, Tower> towers = new Dictionary<TowerTypes, Tower>();

        public bool PurchaseTower(TowerPlatform _platform, TowerTypes _towerType)
        {
            // true - if enough money, false - if not enough money
            bool towerWasBought = Player.instance.PurchaseTower(towers[_towerType]);

            if (towerWasBought)
            {
                Tower newTower = Instantiate(towers[_towerType]);
                _platform.AddTower(newTower);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            towers.Add(TowerTypes.DefaultTower, spawnableTowers[0]);
            towers.Add(TowerTypes.MagicTower, spawnableTowers[1]);
            towers.Add(TowerTypes.SniperTower, spawnableTowers[2]);
            towers.Add(TowerTypes.MachineGunTower, spawnableTowers[3]);

            towerPriceTexts[0].text = "$" + towers[TowerTypes.DefaultTower].Cost;
            towerPriceTexts[1].text = "$" + towers[TowerTypes.MagicTower].Cost;
            towerPriceTexts[2].text = "$" + towers[TowerTypes.SniperTower].Cost;
            towerPriceTexts[3].text = "$" + towers[TowerTypes.MachineGunTower].Cost;
        }
    }

}

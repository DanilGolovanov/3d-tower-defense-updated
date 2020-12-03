using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Towers;

namespace TowerDefence.Manager
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance = null;

        [SerializeField]
        private List<Tower> spawnableTowers = new List<Tower>();

        private List<Tower> aliveTowers = new List<Tower>();

        private Dictionary<TowerTypes, Tower> towers = new Dictionary<TowerTypes, Tower>();

        public void PurchaseTower(TowerPlatform _platform, TowerTypes _towerType)
        {
            Player.instance.PurchaseTower(towers[_towerType]);

            Tower newTower = Instantiate(towers[_towerType]);
            _platform.AddTower(newTower);
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
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}

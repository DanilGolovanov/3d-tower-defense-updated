using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Towers;

namespace TowerDefence
{

    public class Player : MonoBehaviour
    {
        /// <summary>
        /// the reference to the only player gameObject in the game
        /// </summary>
        public static Player instance = null;

        [SerializeField, Tooltip("sets the initial amount of money player has")]
        private int money = 100;

        /// <summary>
        /// gives the player the passed amount of money
        /// </summary>
        /// <param name="_money">game currency</param>
        public void AddMoney(int _money)
        {
            money += _money;
        }
        public void AddMoney(Enemies.Enemy _enemy)
        {
            money += _enemy.Money;
        }

        /// <summary>
        /// handles the removal of money when purchasing a tower and
        /// notifies the TowerManager to place the tower
        /// </summary>
        /// <param name="_tower">the tower being purchased</param>
        public void PurchaseTower(Tower _tower)
        {
            money -= _tower.Cost;
        }

        // Start is called before the first frame update
        void Awake() //awake more often than not goes before start()
        {
            if (instance == null) //if instance doesn't already exist, make it me
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            //this should only happen to the instance
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerDefence.Towers;

//script to define the instance of player 

namespace TowerDefence
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// the reference to the only player gameObject in the game
        /// </summary>
        public static Player instance = null;

        [Tooltip("sets the initial amount of money player has")]
        public int money = 100;
        public Text moneyText;

        /// <summary>
        /// gives the player the passed amount of money
        /// </summary>
        /// <param name="_money">game currency</param>
        public void AddMoney(int _money)
        {
            money += _money;
        }
        public void RemoveMoney(int _money)
        {
            money -= _money;
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
        public bool PurchaseTower(Tower _tower)
        {
            if (money >= _tower.Cost)
            {
                money -= _tower.Cost;
                return true;
            }
            else
            {
                return false;
            }
        }

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
            //DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            moneyText.text = "Money: $" + money.ToString();
        }
        public void TakeDamage(float damage)
        {
            if (GameManager.instance.currentHealth < 0)
            {
                //player dead
                return; //for now
            }
            GameManager.instance.currentHealth -= damage;
        }
    }
}

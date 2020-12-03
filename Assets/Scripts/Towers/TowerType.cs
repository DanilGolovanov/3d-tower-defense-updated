﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Utilities;
using TowerDefence.Enemies;
using TowerDefence.Managers;

namespace TowerDefence.Towers
{
    public enum TowerTypes
    {
        DefaultTower,
        MagicTower,
        SniperTower,
        MachineGunTower
    }

    public class TowerType : Tower
    {
        [SerializeField]
        public TowerTypes towerType;
        [SerializeField]
        private string towerDescription;
        [SerializeField]
        public float damageToGive;
        [SerializeField]
        public float maxHealth;
        [SerializeField]
        public float fireRange;
        [SerializeField]
        public float rechargeTime;
        private float rechargeCount;
        [SerializeField]
        public bool canHitMultipleEnemies;

        // stats which benefit from being bigger
        public float positiveStatMultiplier = 1.2f;
        // stats which benefit from being small
        public float negativeStatMultiplier = 0.85f;

        private Vector3 towerPosition;

        private Enemy[] closeEnemies;
        private EnemyManager enemyManager;
        private Spawner spawner;

        private TowerComponent[] towerComponents;

        public int upgradeCost;
        public int initialUpgradeCost;

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            spawner = FindObjectOfType<Spawner>();
            // get all tower bases and tower top 
            // each time tower is upgraded this script will be instantiated and called again
            towerComponents = GetComponentsInChildren<TowerComponent>();

            // get tower position at the same Y-axis height level as the enemies 
            // in order measure the fireRange horizontally instead taking into account height of the tower
            towerPosition = new Vector3(transform.position.x, spawner.transform.position.y, transform.position.z);

            rechargeCount = rechargeTime;
        }

        private void Update()
        {
            HandleDamageSystem();

            if (maxHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void HandleDamageSystem()
        {
            closeEnemies = enemyManager.GetClosestEnemies(towerPosition, fireRange);
            
            if (rechargeCount <= 0)
            {
                // give damage to each of the enemies within range
                // if the tower type can hit multiple enemies at once
                if (canHitMultipleEnemies)
                {
                    foreach (var enemy in closeEnemies)
                    {
                        enemy.health -= damageToGive;
                    }
                }
                // if tower can't attack multiple enemies, just make damage to one of them
                else
                {
                    if (closeEnemies.Length > 0)
                    {
                        closeEnemies[0].health -= damageToGive;
                    }
                }
                foreach (var towerComponent in towerComponents)
                {
                    towerComponent.PlayAttackVisuals();
                }
                rechargeCount = rechargeTime;
            }
            else
            {
                rechargeCount -= Time.deltaTime;
            }
        }
    }
}
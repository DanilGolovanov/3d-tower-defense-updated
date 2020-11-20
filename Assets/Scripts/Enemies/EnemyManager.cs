using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;

namespace TowerDefence.Managers
{

    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance = null;
        [SerializeField]
        private GameObject enemyPrefab;

        private List<Enemy> aliveEnemies = new List<Enemy>();

        public void SpawnEnemy(Transform _spawner)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, _spawner.position, enemyPrefab.transform.rotation);
            aliveEnemies.Add(newEnemy.GetComponent<Enemy>());
        }

        public void KillEnemy(Enemy _enemy)
        {
            // Attempt to find the enemy in the list
            int enemyIndex = aliveEnemies.IndexOf(_enemy);
            if(enemyIndex != -1)
            {
                // the enemy exists and we can it and remove it from the list
                Destroy(_enemy.gameObject);
                aliveEnemies.RemoveAt(enemyIndex);
            }
        }

        /// <summary>
        /// loops through all eneimes alive in the game, finding the closest enimes within a certain range
        /// </summary>
        /// <param name="_target">the object we are comparing the distance to</param>
        /// <param name="_minRange">the max range we are finding eneimes within</param>
        /// <param name="_maxRange">the range must be at least from target</param>
        /// <returns>the list of eneimes within the given range</returns>
        public Enemy[] GetClosestEnemies(Transform _target, float _maxRange, float _minRange = 0)
        {
            List<Enemy> closeEnemies = new List<Enemy>();

            foreach(Enemy enemy in aliveEnemies)
            {
                //detect if enemy is within the specificed range, if so append to list
                float distance = Vector3.Distance(enemy.transform.position, _target.position);
                if (distance < _maxRange && distance >_minRange)
                {
                    closeEnemies.Add(enemy);
                }
            }
            //converts list to an array
            return closeEnemies.ToArray();
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
        }
    }
}
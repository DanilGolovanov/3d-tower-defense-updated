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

        public Transform[] spawnPoints;
        private List<Enemy> aliveEnemies = new List<Enemy>();

        public void SpawnEnemy(Transform _spawner)
        {
            Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject newEnemy = Instantiate(enemyPrefab, _sp.position, _sp.transform.rotation);
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
        /// loops through all enemies alive in the game, finding the closest es within a certain range
        /// </summary>
        /// <param name="_targetPosition">the object we are comparing the distance to</param>
        /// <param name="_minRange">the max range we are finding eneimes within</param>
        /// <param name="_maxRange">the range must be at least from target</param>
        /// <returns>the list of enemies within the given range</returns>
        public Enemy[] GetClosestEnemies(Vector3 _targetPosition, float _maxRange, float _minRange = 0)
        {
            List<Enemy> closeEnemies = new List<Enemy>();

            foreach(Enemy enemy in aliveEnemies)
            {
                //detect if enemy is within the specificed range, if so append to list
                float distance = Vector3.Distance(enemy.transform.position, _targetPosition);
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
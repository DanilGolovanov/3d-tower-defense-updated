using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{

    public class Spawner : MonoBehaviour
    {
        //properties
        public float SpawnRate
        {
            get
            {
                return spawnRate;
            }
        }
        //variables
        [SerializeField]
        private float spawnRate = 1;

        private float currentTime = 0;

        private EnemyManager enemyManager;

        // Start is called before the first frame update
        void Start()
        {
            enemyManager = EnemyManager.instance;
        }

        // Update is called once per frame
        void Update()
        {
            // Increment the time by delta time if the current time is less than the SpawnRate
            if (currentTime < SpawnRate)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                //Attempt to spawn the enemy via EnemyManager Singleton
                if (enemyManager != null)
                {
                    enemyManager.SpawnEnemy(transform);
                }
            }
        }

        //public int waveNumber;
       // public int enemySpawnAmount;
        //public static int eneimesKilled;
        //void StartWave()
        //{
            //waveNumber = 1;
            //enemySpawnAmount = 2;
            //eneimesKilled = 0;

            //for (int i = 0; i < enemySpawnAmount; i++)
            //{
                //enemyManager.SpawnEnemy(transform);
            //}
        //}
        //void NextWave()
        //{
            //waveNumber++;
            //enemySpawnAmount += 2; //increase spawn amount by varaible
            //eneimesKilled = 0;

            //for (int i = 0; i < enemySpawnAmount; i++)
            //{
                //enemyManager.SpawnEnemy(transform);
            //}
        //}
    }
}
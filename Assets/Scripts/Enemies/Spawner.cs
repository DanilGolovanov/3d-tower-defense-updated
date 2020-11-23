using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{
    public class Spawner : MonoBehaviour
    {
        public enum SpawnPhase { SPAWNING, WAITING, COUNTING }

        [System.Serializable]
        public class Wave
        {
            public string name;
            public Transform enemy;
            public int numberOfEnemies;
            public float delayBetweenEnemySpawn;
        }
        public Wave[] waves;
        private int nextWave = 0;
        public float timeBetweenWaves = 5;
        private float wavesCountDown;
        private float enemyCheck = 1f;

        public SpawnPhase state = SpawnPhase.COUNTING;
        private EnemyManager enemyManager;

        void Start()
        {
            wavesCountDown = timeBetweenWaves;
            enemyManager = EnemyManager.instance;
        }

        void Update()
        {
            if (state == SpawnPhase.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    WaveCompleted();                
                }
                else
                {
                    return;
                }
            }

            if (wavesCountDown <= 0)
            {
                if (state != SpawnPhase.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                wavesCountDown -= Time.deltaTime;
            }
        }

        IEnumerator SpawnWave(Wave _wave)
        {
            state = SpawnPhase.SPAWNING;

            for (int i = 0; i < _wave.numberOfEnemies; i++)
            {
                enemyManager.SpawnEnemy(transform);
                yield return new WaitForSeconds(1f / _wave.delayBetweenEnemySpawn);
            }

            state = SpawnPhase.WAITING;

            yield break;
        }

        void WaveCompleted()
        {
            state = SpawnPhase.COUNTING;
            wavesCountDown = timeBetweenWaves;

            if (nextWave + 1 > waves.Length - 1)
            {
                //call game over here
                //temp code for testing loops back to 1st wave
                nextWave = 0;
                Debug.Log("All waves finished");
            }
            else
            {
                nextWave++;
            }         
        }
        //checking if enemy are still active on heirachry once per second instead of every frame
        bool EnemyIsAlive()
        {
            enemyCheck -= Time.deltaTime;
            if (enemyCheck < 0f)
            {
                enemyCheck = 1f;
                if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
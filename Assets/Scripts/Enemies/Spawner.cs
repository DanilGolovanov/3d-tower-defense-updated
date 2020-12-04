using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TowerDefence.Managers;

//wave based system

namespace TowerDefence.Enemies
{
    public class Spawner : MonoBehaviour
    {
        //wave states
        public enum SpawnPhase { SPAWNING, WAITING, COUNTING }

        [System.Serializable]
        public class Wave
        {
            [Header ("Wave Calibration")]
            public string name;
            public Transform enemy;
            public int numberOfEnemies;
            public float delayBetweenEnemySpawn;
        }
        public Wave[] waves;
        private int nextWave = 1;
        public float timeBetweenWaves = 5;
        private float wavesCountDown;
        private float enemyCheck = 1f;
        //references
        public SpawnPhase state = SpawnPhase.COUNTING;
        private EnemyManager enemyManager;
        public Text waveNotification;
        public Text waveState;

        //sky colours
        public Color32 nonWave = new Color32(120, 120, 120, 255);
        public Color32 inWave = new Color32(140, 1, 0, 255);
        public float duration = 5;
        public Light directionalLight;
        public Color32 nonWaveLight = new Color32(255, 255, 255, 255);
        public Color32 inWaveLight = new Color32(255, 148, 148, 255);

        //audio
        public AudioClip[] waveAmbiance;
        private AudioSource audioSource;
        private AudioListener audioListener;


        void Start()
        {
            directionalLight = directionalLight.GetComponent<Light>();
            wavesCountDown = timeBetweenWaves;
            enemyManager = EnemyManager.instance;
            audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
            audioSource = GameObject.FindGameObjectWithTag("Ambiance").GetComponent<AudioSource>();
        }
        //check if enimes are alive in waiting state
        //complete wave if all are dead
        //switch to spawning state if countdown timer is over
        //change atmosphere during wave
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
                    waveState.text = "Base is under attack!";
                    return;     
                }
            }

            if (wavesCountDown <= 0)
            {
                if (state != SpawnPhase.SPAWNING)
                {
                    waveState.text = "Enemies are incoming!";
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                wavesCountDown -= Time.deltaTime;
            }

            waveNotification.text = "Wave " + nextWave.ToString();
            SkyChange();
        }
        //spawn enemies over time
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
        //switch state to count down
        //notfiy player
        //call win condition if passed wave 10
        void WaveCompleted()
        {
            state = SpawnPhase.COUNTING;

            waveState.text = "Time to restock and repair!";

            wavesCountDown = timeBetweenWaves;

            if (nextWave + 1 > waves.Length - 1)
            {
                SceneManager.LoadScene(5);
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
        //change atmosphere of game world during wave
        private void SkyChange()
        {
            if (state == SpawnPhase.COUNTING)
            {

                RenderSettings.skybox.SetColor("_Tint", nonWave);
                directionalLight.color = nonWaveLight;
                RenderSettings.fog = false;
                if (audioSource.isPlaying)
                {
                    StopRandomWave();
                }
            }

            if (state == SpawnPhase.SPAWNING)
            {
                RenderSettings.skybox.SetColor("_Tint", inWave);
                directionalLight.color = inWaveLight;
                RenderSettings.fog = true;
                if (!audioSource.isPlaying)
                {
                    PlayRandomWave();
                }
            }
        }
        void PlayRandomWave()
        {
            audioSource.clip = waveAmbiance[Random.Range(0, waveAmbiance.Length)];
            audioSource.Play();
        }
        void StopRandomWave()
        {
            audioSource.Stop();
        }
    }
}
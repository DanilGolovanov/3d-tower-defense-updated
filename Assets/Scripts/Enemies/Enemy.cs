using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerDefence.Towers;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{

    public class Enemy : MonoBehaviour
    {
        [System.Serializable]
        public class DeathEvent : UnityEvent<Enemy>
        {

        }
        public float XP { get { return xp; } }
        public int Money { get { return money; } }

        [Space]

        [SerializeField]
        private DeathEvent onDeath = new DeathEvent();

        [Header("General Stats")]
        [SerializeField, Tooltip("how fast the enemy will move")]
        private float speed = 1;
        [Tooltip("how much damage enemy can take before dying")]
        public float health = 1;
        [SerializeField, Tooltip("damage to player health")]
        private float damage = 1;
        [SerializeField, Tooltip("how big is the enemy visually")]
        private float size = 1;
        //resistance here

        [Header("Rewards")]
        [SerializeField, Tooltip("The amount of xp tower gets killing an enemy")]
        private float xp = 1;
        [SerializeField, Tooltip("The amount of money tower gets killing an enemy")]
        private int money = 1;

        private Player player; //reference to player game object within the scene
        private EnemyManager enemyManager;

        /// <summary>
        /// Handles damage of the enemy and if below or equal to 0 calls Die()
        /// </summary>
        /// <param name="_tower">The tower doing damage to the enemy</param>
        public void Damage(float _damage)
        {
            health -= _damage;
            if (health <= 0)
            {
                Die();
            }
        }
        /// <summary>
        /// Handles the visual and technical features of dying such as giving tower xp
        /// </summary>
        /// <param name="_tower">the tower that killed the enemy</param>
        public void Die()
        {
            onDeath.Invoke(this);
            //Visuals
        }

        // Start is called before the first frame update
        void Start()
        {
            //singleton - accessing the only player in game
            player = Player.instance;
            enemyManager = EnemyManager.instance;
            onDeath.AddListener(player.AddMoney);
            onDeath.AddListener(enemyManager.KillEnemy);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using TowerDefence.Towers;
using TowerDefence.Managers;

//script to define enemies, FSM to control states and animation

namespace TowerDefence.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [System.Serializable]
        public class DeathEvent : UnityEvent<Enemy>{}
        //properties
        public float XP { get { return xp; } }
        public int Money { get { return money; } }
        //event for enemy death
        [SerializeField]
        private DeathEvent onDeath = new DeathEvent();
        //FSM states for AI
        public enum EnemyState
        {
            PATROL,
            CHASE,
            ATTACK,
            ATTACKBASE,
            DYING
        }
        [Header("AI Finate State Machine")]
        public EnemyState enemyState;
        public float patrolSpeed = 0.5f;
        public float attackSpeed = 4f;
        public float chaseDistance = 7f;
        private float currentChaseDistance;
        public float attackDistance = 3f;
        public float baseAttackDistance = 10f;
        public float chaseAfterAttackDistance = 2f;
        public float waitBeforeAttack = 2f;
        private float attackTimer;
        private bool dead;

        [Header("General Stats")]
        [Tooltip("how much damage enemy can take before dying")]
        public float health = 1;

        [Header("Rewards")]
        [SerializeField, Tooltip("The amount of xp tower gets killing an enemy")]
        private float xp = 1;
        [SerializeField, Tooltip("The amount of money tower gets killing an enemy")]
        private int money = 1;

        //enemy audio
        public AudioClip[] enemyAttackAudio;
        public AudioClip[] enemyDeathAudio;
        private AudioSource audioSource;
        private AudioListener audioListener;

        //references
        private Player player; //reference to player game object within the scene
        private EnemyManager enemyManager;
        private Animator enemyAnim;
        private Transform enemyTransform;
        private Transform target;
        private Transform targetBase;
        private NavMeshAgent navAgent;
        public MainBase mainBase;
        public GameObject attack_Point;

        public GameObject bloodSplat;


        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            enemyAnim = GetComponent<Animator>();
            mainBase = GameObject.FindGameObjectWithTag("Base").GetComponent<MainBase>();
            enemyTransform = transform; //assign the reference of Transform
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetBase = GameObject.FindGameObjectWithTag("Base").transform;
            audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            player = Player.instance;
            enemyManager = EnemyManager.instance;
            onDeath.AddListener(player.AddMoney);
            onDeath.AddListener(enemyManager.KillEnemy);
            enemyAnim.SetBool("isRunning", true);

            enemyState = EnemyState.PATROL;
            // when the enemy first gets to the player - attack right away
            attackTimer = waitBeforeAttack;
            // memorize the value of chase distance
            currentChaseDistance = chaseDistance;
            dead = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (enemyState == EnemyState.PATROL)
                {
                    Patrol();
                }

            if (enemyState == EnemyState.CHASE)
                {
                    Chase();
                }

            if (enemyState == EnemyState.ATTACK)
                {
                    Attack();
                }

            if (enemyState == EnemyState.ATTACKBASE)
                {
                    AttackBase();
                }

            if (enemyState == EnemyState.DYING)
                {
                    Dying();
                }
            
            Debug.Log(enemyState);

        }
        public EnemyState Enemy_State
        {
            get; set;
        }
        void Patrol()
        {
            // nav agent can move
            navAgent.isStopped = false;
            navAgent.speed = patrolSpeed;
            //FollowWaypoint();
            navAgent.SetDestination(targetBase.position);

            if (navAgent.velocity.sqrMagnitude > 0)
            {
                enemyAnim.SetBool("isRunning", true);
            }
            else
            {
                enemyAnim.SetBool("isRunning", false);
            }
            // test the distance between the player and the enemy
            if (Vector3.Distance(transform.position, target.position) <= chaseDistance)
            {
                enemyState = EnemyState.CHASE;
            }
            if (Vector3.Distance(transform.position, targetBase.position) <= 4.5f)
            {
                enemyState = EnemyState.ATTACKBASE;
            }
        }

        void Chase()
        {
            enemyAnim.SetBool("isRunning", true);
            // enable the agent to move again
            navAgent.isStopped = false;
            navAgent.speed = attackSpeed;
            // set the player's position as the destination
            navAgent.SetDestination(target.position);
            // if the distance between enemy and player is less than attack distance
            if (Vector3.Distance(transform.position, target.position) <= attackDistance)
            {
                enemyState = EnemyState.ATTACK;
                // reset the chase distance to previous
                if (chaseDistance != currentChaseDistance)
                {
                    chaseDistance = currentChaseDistance;
                }
            }
            else if (Vector3.Distance(transform.position, target.position) > chaseDistance)
            {
                enemyState = EnemyState.PATROL;
                // reset the chase distance to previous
                if (chaseDistance != currentChaseDistance)
                {
                    chaseDistance = currentChaseDistance;
                }
            } 
        }
        //basic player attack
        void Attack()
        {
            //stop agent moving
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            attackTimer += Time.deltaTime;
            //check attack cooldown
            //and attack if possible
            if (attackTimer > waitBeforeAttack)
            {
                enemyAnim.SetBool("isAttacking", true);
                attackTimer = 0f;
                PlayEnemyAttackAudio();

            }
            //if player moves out of attack range, chase
            if (Vector3.Distance(transform.position, target.position) >
               attackDistance + chaseAfterAttackDistance)
            {
                enemyAnim.SetBool("isAttacking", false);
                enemyState = EnemyState.CHASE;
            }

        }
        //attack base
        void AttackBase()
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            attackTimer += Time.deltaTime;

            if (attackTimer > waitBeforeAttack)
            {
                enemyAnim.SetBool("isAttacking", true);
                attackTimer = 0f;
                PlayEnemyAttackAudio();

            }
        }
        void Dying()
        {
            dead = true;
            navAgent.velocity = Vector3.zero;
            navAgent.enabled = false;
            disableHitDetection();
            StartCoroutine(Die());
        }
        /// <summary>
        /// Handles the visual and technical features of dying
        /// </summary>
        IEnumerator Die()
        {
            enemyAnim.SetBool("isRunning", false);
            enemyAnim.SetBool("isDead", true);
            yield return new WaitForSeconds(5f);
            onDeath.Invoke(this);
        }
        /// <summary>
        /// Handles damage of the enemy and if below or equal to switches enum state to dying
        /// </summary>
        public void Damage(float _damage)
        {
            health -= _damage;
            if (health <= 0)
            {
                enemyState = EnemyState.DYING;
            }
        }
        //enemy attack point (to toggle on with animation event)
        void Turn_On_AttackPoint()
        {
            attack_Point.SetActive(true);
        }

        void Turn_Off_AttackPoint()
        {
            if (attack_Point.activeInHierarchy)
            {
                attack_Point.SetActive(false);
            }
        }
        //disable hit boxes on death
        void disableHitDetection()
        {
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }
        }
        //enemy audio
        void PlayEnemyAttackAudio()
        {
            audioSource.clip = enemyAttackAudio[Random.Range(0, enemyAttackAudio.Length)];
            audioSource.Play();
        }
        void PlayEnemyDeathAudio()
        {
            audioSource.clip = enemyDeathAudio[Random.Range(0, enemyDeathAudio.Length)];
            audioSource.Play();
        }
        //enemy effects
        void PlayEnemyBloodSplat()
        {
            Instantiate(bloodSplat, transform.position, Quaternion.identity);
        }
    }
}
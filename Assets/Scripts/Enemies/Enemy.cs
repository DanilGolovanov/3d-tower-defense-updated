using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using TowerDefence.Towers;
using TowerDefence.Managers;

namespace TowerDefence.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [System.Serializable]
        public class WaypointInfo
        {
            public Vector3 wayPoint;
            public bool IsWaypointReached(Vector3 movingObject, float deadZone = 0.3f)
            {
                if (Vector3.Distance(movingObject, wayPoint) < deadZone)
                {
                    return true;
                }
                return false;
            }
        }
        [System.Serializable]
        public class DeathEvent : UnityEvent<Enemy>{}

        //properties
        public float XP { get { return xp; } }
        public int Money { get { return money; } }

        //event for enemy death
        [SerializeField]
        private DeathEvent onDeath = new DeathEvent();

        [Header("Waypoint System")]
        public float turnSpeed = 5;
        public WaypointInfo[] wayPoints;
        private WaypointInfo currentWayPoint;
        private int currentWayPointIndex;

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
        //private EnemyAudio enemy_Audio;

        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            enemyAnim = GetComponent<Animator>();
            mainBase = GameObject.FindGameObjectWithTag("Base").GetComponent<MainBase>();
            enemyTransform = transform; //assign the reference of Transform
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetBase = GameObject.FindGameObjectWithTag("Base").transform;

            if (wayPoints.Length > 0)
            {
                currentWayPoint = wayPoints[0];//set initial waypoint
                currentWayPointIndex = 0;
            }
            else
            {
                Debug.LogError("No waypoint assigned");
            }
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
                // play spotted audio
                //enemy_Audio.Play_ScreamSound();
            }
            if (Vector3.Distance(transform.position, targetBase.position) <= 4.5f)
            {
                enemyState = EnemyState.ATTACKBASE;
                // play spotted audio
                //enemy_Audio.Play_ScreamSound();
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

        void Attack()
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            attackTimer += Time.deltaTime;

            if (attackTimer > waitBeforeAttack)
            {
                enemyAnim.SetBool("isAttacking", true);
                attackTimer = 0f;
                // play attack sound
                //enemy_Audio.Play_AttackSound();

            }
            if (Vector3.Distance(transform.position, target.position) >
               attackDistance + chaseAfterAttackDistance)
            {
                enemyAnim.SetBool("isAttacking", false);
                enemyState = EnemyState.CHASE;
            }

        }
        void AttackBase()
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            attackTimer += Time.deltaTime;

            if (attackTimer > waitBeforeAttack)
            {
                enemyAnim.SetBool("isAttacking", true);
                attackTimer = 0f;
                // play attack sound
                //enemy_Audio.Play_AttackSound();

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
        void FollowWaypoint()
        {
            if (!dead)
            {
                //Turning the object to the target
                enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, Quaternion.LookRotation(currentWayPoint.wayPoint - enemyTransform.position), Time.deltaTime * turnSpeed);

                enemyTransform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
                if (currentWayPoint.IsWaypointReached(enemyTransform.position))
                {
                    NextWaypoint();
                }
            }
        }
        void NextWaypoint()
        {
            currentWayPointIndex++; // try to increase the index
            if (currentWayPointIndex > wayPoints.Length - 1)
            {
                currentWayPointIndex = 0; // if index is larger than list of waypoints, reset it to zero
            }

            currentWayPoint = wayPoints[currentWayPointIndex]; // assign current waypoint from the list
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
        void disableHitDetection()
        {
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }
        }
    }
}
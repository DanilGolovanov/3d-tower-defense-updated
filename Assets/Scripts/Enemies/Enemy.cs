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

        [Header("Waypoint System")]
        public float moveSpeed = 5;
        public float turnSpeed = 5;
        public WaypointInfo[] wayPoints;
        private WaypointInfo currentWayPoint;
        private int currentWayPointIndex;
        private Transform enemyTransform;


        [Header("Rewards")]
        [SerializeField, Tooltip("The amount of xp tower gets killing an enemy")]
        private float xp = 1;
        [SerializeField, Tooltip("The amount of money tower gets killing an enemy")]
        private int money = 1;

        private Player player; //reference to player game object within the scene
        private EnemyManager enemyManager;
        Animator anim;
        Rigidbody rB;
        private bool dead = false;

        /// <summary>
        /// Handles damage of the enemy and if below or equal to 0 calls Die()
        /// </summary>
        /// <param name="_tower">The tower doing damage to the enemy</param>
        public void Damage(float _damage)
        {
            health -= _damage;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
        /// <summary>
        /// Handles the visual and technical features of dying
        /// </summary>
        IEnumerator Die()
        {
            dead = true;
            rB.velocity = Vector3.zero;
            anim.SetBool("isRunning", false);
            anim.SetBool("isDead", true);
            yield return new WaitForSeconds(10f);
            onDeath.Invoke(this);
        }
        void Awake()
        {
            enemyTransform = transform; //assign the reference of Transform
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
            anim = GetComponent<Animator>();
            rB = GetComponent<Rigidbody>();
            player = Player.instance;
            enemyManager = EnemyManager.instance;
            onDeath.AddListener(player.AddMoney);
            onDeath.AddListener(enemyManager.KillEnemy);
            anim.SetBool("isRunning", true);
        }

        // Update is called once per frame
        void Update()
        {
            if (!dead)
            {
                //Turning the object to the target
                enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, Quaternion.LookRotation(currentWayPoint.wayPoint - enemyTransform.position), Time.deltaTime * turnSpeed);

                enemyTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Enemies;
using TowerDefence.Managers;

namespace TowerDefence.Towers
{

    public abstract class Tower : MonoBehaviour
    {
        #region examplecode
        /*public string towerName = "Tower"; //adding a title to unity editor as string varaible
        [Multiline] // adds a box instead of one line for unity editor
        public string description = "This is a descrption";

        [SerializeField] // allows variable to be adjusted on unity editor without being public
        [Header("Properties")] //adds a header on inspector
        [Space] // adds space on inspector
        [Range(-3, 3)] // locks range for variable
        [Min(0)] //locks a minimum for variable
        [Tooltip("This variable adjusts speed which controls how fast object moves up or down")] // adds tool tip to inspector
        private float speed;*/
        #endregion
        #region Properties
        public string TowerName // the public accessor for TowerName variable
        {
            get //two line return
            {
                return towerName;
            }
        }

        public string Description // the public accessor for Description variable
        {
            get => description; // one line return
        }

        public int Cost // the public accessor for cost variable
        {
            get => cost;
        }

        /// <summary>
        /// Gets a formatted string containing all of the description, tower properties
        /// name and cost to be displayed on UI.
        /// </summary>
        public string UiDisplayText
        {
            get // To.String is not nessessary due to varaibles being passed through via {} -- these format as strings anyway
            {
                string display = string.Format("Name: {0} Cost: {1}\n", TowerName, Cost.ToString());
                display += Description + "\n";
                display += string.Format("Min Range: {0}, Max Range: {1}, Damage: {2}", minimumRange.ToString(), maximumRange.ToString(), Damage.ToString());
                return display;
            }
        }
        /// <summary>
        /// calculates required experience based on current level
        /// and the experience scaler.
        /// </summary>
        private float RequiredXp
        {
            get
            {
                if (level == 1) //if level is equal to 1
                {
                    return baseRequiredXp; //return base required xp
                }
                return baseRequiredXp * (level * experienceScaler); // multiply the level by the experienceScaler to get the multiplier for the baseRequiredXp
            }
        }
        /// <summary>
        /// the maximum range that the tower can reach based on it's level
        /// </summary>
        public float MaximumRange
        {
            get
            {
                return maximumRange * (level * 0.5f + 0.5f);
            }
        }
        /// <summary>
        /// the amount of damage that the tower does, multipled by its level
        /// </summary>
        public float Damage
        {
            get
            {
                return damage * (level * 0.5f + 0.5f);
            }
        }

        protected Enemy TargetedEnemy
        {
            get
            {
                return target;
            }
        }

        #endregion

        [Header("General Stats")]
        [SerializeField]
        private string towerName = "";
        [SerializeField, TextArea]
        private string description = "";
        [SerializeField, Range(1, 10), Tooltip("Adjust cost between 1 and 10")]
        private int cost = 1;

        [Header("Attack Stats")]
        [SerializeField, Min(0.1f), Tooltip("Adjust Damage")]
        public float damage = 1;
        [SerializeField, Min(0.1f), Tooltip("Adjust Minimum Range")]
        private float minimumRange = 1;
        [SerializeField, Tooltip("Adjust Maximum Range")]
        private float maximumRange = 5;
        [SerializeField, Min(0.1f), Tooltip("Adjust Fire rate")]
        protected float fireRate = 0.1f;

        [Header("Experience Stats")]
        [SerializeField, Range(2, 5), Tooltip("Adjust Range between 2 and 5")]
        private int maxLevel = 3;
        [SerializeField, Min(1)]
        private float baseRequiredXp = 5;
        [SerializeField, Min(1)]
        private float experienceScaler = 1;

        private int level = 1;
        private float xp = 0;
        private Enemy target = null;

        private float currentTime = 0;


#if UNITY_EDITOR
        private void OnValidate() //only runs in editor - triggers whenever variable change made in inspector of this class
        {
            maximumRange = Mathf.Clamp(maximumRange, minimumRange + 1, float.MaxValue);
        }


        private void OnDrawGizmosSelected() //draws gizmo (gizmo = debug visuals) around visuals only when selected
        {
            //draw transparent red sphere indicating the minimum range
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(transform.position, minimumRange);

            //draw transparent blue sphere indicating maximum range
            Gizmos.color = new Color(0, 0, 1, 0.25f);
            Gizmos.DrawSphere(transform.position, maximumRange);
        }
#endif

        public void AddExperience(Enemy _enemy)
        {
            //check that level is not max and that we have passed required experience to level up
            xp += _enemy.XP; 
            if (level < maxLevel)
            {
                if (xp >= RequiredXp)
                {
                    LevelUp();
                }
            }
        }


        protected abstract void RenderAttackVisuals();
        protected abstract void RenderLevelUpVisuals();


        private void Fire()
        {
            //make sure there is something to target
            //if there is, damage it
            if (target != null)
            {
                target.Damage(Damage);
                RenderAttackVisuals();
            }
        }

        private void FireWhenReady()
        {
            //make sure there is a target
            if (target != null)
            {
                //if the timer is less than fire rate
                if (currentTime < fireRate)
                {
                    //add delta time to make sure tower fires in real time
                    currentTime += Time.deltaTime;
                }
                else
                {
                    //reset the current time and call fire method
                    currentTime = 0;
                    Fire();
                }
            }
        }

        private void LevelUp()
        {
            level++;
            xp = 0;

            RenderLevelUpVisuals(); //display level up visuals here
        }

        private void Target()
        {
            //get enemies within range
            Enemy[] closeEnemies = EnemyManager.instance.GetClosestEnemies(transform, MaximumRange, minimumRange);
            //GetClosestEnemy();
            target = GetClosestEnemy(closeEnemies);
        }

        // _enemies is the array of closest enemies within range
        private Enemy GetClosestEnemy(Enemy[] _enemies)
        {
            //distance between us and enemy 
            float closestDist = float.MaxValue;
            Enemy closest = null;

            foreach (Enemy enemy in _enemies)
            {
                //if enemy is closer than the current make it the closest
                float distToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                if (distToEnemy < closestDist)
                {
                    closestDist = distToEnemy;
                    closest = enemy;
                }
            }
            return closest;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Target();
            FireWhenReady();
            //mapping arrows keys to go up, down, left and right
            /*transform.position += transform.up * Time.deltaTime * speed * Input.GetAxis("Vertical");
            transform.position += transform.right * Time.deltaTime * speed * Input.GetAxis("Horizontal");*/
        }

        /*private void OnDrawGizmos() //only works in scene editor
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position + transform.up, Vector3.one);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + transform.right, Vector3.one);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position + transform.up, transform.position + transform.right);*/
    }

}
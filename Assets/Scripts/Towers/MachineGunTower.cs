using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Utilities;

namespace TowerDefence.Towers 
{ 

    public class MachineGunTower : Tower
    {

        [Header("Machine Gun Specifics")]
        [SerializeField]
        private Transform turret;
        [SerializeField]
        private Transform gunHolder;
        [SerializeField]
        private LineRenderer bulletLine;
        [SerializeField]
        private Transform leftFirePoint;
        [SerializeField]
        private Transform rightFirePoint;

        private bool fireLeft = false;

        private float resetTime = 0;
        private bool hasResetVisuals = false;

        protected override void RenderAttackVisuals()
        {
            MathUtils.DistanceAndDirection(out float distance, out Vector3 direction, gunHolder, TargetedEnemy.transform);
            gunHolder.rotation = Quaternion.LookRotation(direction);
            if (fireLeft)
            {
                RenderBulletLine(leftFirePoint);
            }
            else
            {
                RenderBulletLine(rightFirePoint);
            }
            fireLeft = !fireLeft;
            hasResetVisuals = false;
        }

        protected override void RenderLevelUpVisuals()
        {
            Debug.Log("I am leveling up");
        }

        private void RenderBulletLine(Transform _start)
        {
            //spawns a line with two points from the start to the targetted enemy
            bulletLine.positionCount = 2;
            bulletLine.SetPosition(0, _start.position);
            bulletLine.SetPosition(1, TargetedEnemy.transform.position);
        }
        protected override void Update()
        {
            base.Update();

            // detect if we have no enemy AND that we havent already reset the visuals
                // check if the current time is less than the fire rate
                    // add to the current time
                // disable line renderer
                // reset timer to 0
                // set reset visuals flag to true
            if (TargetedEnemy == null && !hasResetVisuals)
            {
                if (resetTime < fireRate)
                {
                    resetTime += Time.deltaTime;
                }
                else
                {
                    bulletLine.positionCount = 0;
                    resetTime = 0;
                    hasResetVisuals = true;
                }
            }

        }
    }
}

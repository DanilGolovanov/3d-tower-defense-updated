using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Manager;

namespace TowerDefence.Towers
{
    public class TowerPlatform : MonoBehaviour
    {
        [SerializeField]
        private Transform towerHolder;
        private Tower heldTower;

        private void OnMouseUpAsButton()
        {
            TowerManager.instance.PurchaseTower(this);
        }

        public void AddTower(Tower _tower)
        {
            heldTower = _tower;

            _tower.transform.SetParent(towerHolder);
            _tower.transform.localPosition = Vector3.zero;
        }

    }
}

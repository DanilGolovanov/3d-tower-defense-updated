using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;
using TowerDefence;

public class EnemyAttackScript : MonoBehaviour 
{

    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update () {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0) 
        {
            hits[0].gameObject.GetComponent<Player>().TakeDamage(damage);

            gameObject.SetActive(false);

        }

	}

}





























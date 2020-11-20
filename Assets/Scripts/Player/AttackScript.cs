using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;

public class AttackScript : MonoBehaviour 
{

    public float damage = 2f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update () {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0) 
        {
            hits[0].gameObject.GetComponent<Enemy>().Damage(damage);

            //gameObject.SetActive(false);

        }

	}

}





























using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;

public class AttackScript : MonoBehaviour 
{

    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;
    public GameObject bloodSplat;

    void Update () {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0) 
        {
            hits[0].gameObject.GetComponent<Enemy>().Damage(damage);
            Instantiate(bloodSplat, hits[0].transform.position, Quaternion.identity);

            gameObject.SetActive(false);

        }

	}

}





























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

    public AudioClip[] playerHitSounds;
    private AudioSource audioSource;
    private AudioListener audioListener;

    private void Start()
    {
        audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }
    void Update () {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0) 
        {
            hits[0].gameObject.GetComponent<Enemy>().Damage(damage);
            Instantiate(bloodSplat, hits[0].transform.position, Quaternion.identity);
            audioSource.Play();

            gameObject.SetActive(false);
        }

	}
    void PlayPlayerHit()
    {
        audioSource.clip = playerHitSounds[Random.Range(0, playerHitSounds.Length)];
        audioSource.Play();
    }

}





























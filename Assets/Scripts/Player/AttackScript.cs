using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;

//melee attack script

public class AttackScript : MonoBehaviour 
{
    [Header ("Melee Calibration")]
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;
    public GameObject bloodSplat;
    //melee audio
    public AudioClip[] playerHitSounds;
    private AudioSource audioSource;
    private AudioListener audioListener;

    private void Start()
    {
        audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }
    //check if attack point game object has collided with anything while active
    //if so, pass through damage
    //instantiate blood effects and play audio
    //toggle attack point off
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





























using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;
using TowerDefence;

//enemy attack script

public class EnemyAttackScript : MonoBehaviour 
{
    [Header("Enemy melee attack calibration")]
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;
    //audio for enemy attack
    public AudioClip[] hitPlayerAudio;
    public AudioClip[] hitBaseAudio;
    private AudioSource audioSource;
    private AudioListener audioListener;

    private void Start()
    {
        audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }
    //check if attack point hit something
    //check if something is base or player
    //pass through damage
    //shake camera if player hit
    //play audio
    //reset attack point
    void Update () 
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0) 
        {
            if (hits[0].gameObject.CompareTag("Player"))
            {
                hits[0].gameObject.GetComponent<Player>().TakeDamage(damage);
                CameraShake.Shake(0.25f, 4f);
                if (!audioSource.isPlaying)
                {
                    PlayRandomPlayerHit();
                }
            }
            else if (hits[0].gameObject.CompareTag("Base"))
            {
                hits[0].gameObject.GetComponent<MainBase>().TakeDamage(damage);
                //base hit effect here
                if (!audioSource.isPlaying)
                {
                    PlayRandomBaseHit();
                }
            }
            else
            {
                return;
                //do tower damage here
            }
            //reset attack point
            gameObject.SetActive(false);

        }

	}
    void PlayRandomPlayerHit()
    {
        audioSource.clip = hitPlayerAudio[Random.Range(0, hitPlayerAudio.Length)];
        audioSource.Play();
    }
    void PlayRandomBaseHit()
    {
        audioSource.clip = hitBaseAudio[Random.Range(0, hitBaseAudio.Length)];
        audioSource.Play();
    }

}





























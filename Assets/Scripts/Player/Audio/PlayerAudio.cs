using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource footSteps;

    [SerializeField]
    private AudioClip[] footStepSounds;

    private CharacterController charControl;

    [HideInInspector]
    public float volume_Min, volume_Max;

    private float accumulated_Distance;

    [HideInInspector]
    public float step_Distance;

    void Awake()
    {
        footSteps = GetComponent<AudioSource>();

        charControl = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        PlayFootStepSounds();
    }

    void PlayFootStepSounds()
    {
        // if NOT on the ground
        if (!charControl.isGrounded)
            return;

        if (charControl.velocity.sqrMagnitude > 0)
        {
            // accumulated distance is the value how far can we go 
            // until the footstep sound will play
            accumulated_Distance += Time.deltaTime;

            if (accumulated_Distance > step_Distance)
            {

                footSteps.volume = Random.Range(volume_Min, volume_Max);
                footSteps.clip = footStepSounds[Random.Range(0, footStepSounds.Length)];
                footSteps.Play();

                accumulated_Distance = 0f;

            }
        }
        else
        {
            accumulated_Distance = 0f;
        }
    }
}

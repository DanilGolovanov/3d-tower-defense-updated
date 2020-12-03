using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 originalPos;
    private float timeAtCurrentFrame;
    private float timeAtLastFrame;
    private float fakeDelta;

    public AudioClip[] playerHitAudio;
    private AudioSource audioSource;
    private AudioListener audioListener;

    void Awake()
    {
        instance = this;
        audioListener = GameObject.FindGameObjectWithTag("FPSCamera").GetComponent<AudioListener>();
        audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Calculate a fake delta time to Shake even if game was paused.
        timeAtCurrentFrame = Time.realtimeSinceStartup;
        fakeDelta = timeAtCurrentFrame - timeAtLastFrame;
        timeAtLastFrame = timeAtCurrentFrame;
    }

    public static void Shake(float duration, float amount)
    {
        instance.originalPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + Random.insideUnitSphere * amount, Time.deltaTime * 3);

            duration -= fakeDelta;

            yield return null;
        }
        PlayPlayerHitAudio();
        transform.localPosition = originalPos;
    }
    void PlayPlayerHitAudio()
    {
        audioSource.clip = playerHitAudio[Random.Range(0, playerHitAudio.Length)];
        audioSource.Play();
    }
}

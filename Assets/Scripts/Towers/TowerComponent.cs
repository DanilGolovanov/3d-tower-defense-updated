using System.Collections;
using System.Collections.Generic;
using TowerDefence.Towers;
using UnityEngine;

public class TowerComponent : MonoBehaviour
{
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayAttackVisuals()
    {
        particles.Play();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float currentHealth = 100;
    public float maxHealth = 100;
    public static float currentStamina = 100;
    public static float maxStamina = 100;

    public float currentbaseHitPoints = 100;
    public float maxBaseHitPoints = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}

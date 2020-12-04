using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script to handle player health and stamina onto the UI
public class PlayerStats : MonoBehaviour
{
    public Image healthBar;
    public Image staminaBar;

    void Update()
    {
        HealthChange();
        StaminaChange();
    }
    void HealthChange()
    {
        float amount = Mathf.Clamp01(GameManager.instance.currentHealth / GameManager.instance.maxHealth);
        healthBar.fillAmount = amount;
    }
    void StaminaChange()
    {
        float amount = Mathf.Clamp01(GameManager.currentStamina / GameManager.maxStamina);
        staminaBar.fillAmount = amount;
    }
}

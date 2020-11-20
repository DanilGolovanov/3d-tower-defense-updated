using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        float amount = Mathf.Clamp01(GameManager.instance.currentStamina / GameManager.instance.maxStamina);
        staminaBar.fillAmount = amount;
    }
}

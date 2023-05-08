using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarManager : MonoBehaviour
{
    [SerializeField]
    private HealthSO
        protagonistHealth = default; //the HealthBar is watching this object, which is the health of the player

    [SerializeField] private HealthConfigSO healthConfig = default;
    [SerializeField] private Slider healthSlider = default;

    [Header("Listening to")] [SerializeField]
    private VoidEventChannelSO UIUpdateNeeded = default; //The player's Damageable issues this

    private void OnEnable()
    {
        UIUpdateNeeded.OnEventRaised += UpdateHeart;

        InitializeHealthBar();
    }

    private void OnDestroy()
    {
        UIUpdateNeeded.OnEventRaised -= UpdateHeart;
    }

    private void InitializeHealthBar()
    {
        protagonistHealth.SetMaxHealth(healthConfig.InitialHealth);
        protagonistHealth.SetCurrentHealth(healthConfig.InitialHealth);

        UpdateHeart();
    }

    private void UpdateHeart()
    {
        int maxHealth = protagonistHealth.MaxHealth;
        float currentHealth = protagonistHealth.CurrentHealth;

        currentHealth = ((float)protagonistHealth.CurrentHealth -
                         (float)maxHealth * (float)currentHealth / (float)maxHealth);

        healthSlider.value = currentHealth;
    }
}
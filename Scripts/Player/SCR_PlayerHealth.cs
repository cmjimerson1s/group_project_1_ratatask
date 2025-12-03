using System;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class SCR_PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int regenarationAmount;
    private int health;
    private bool shouldTakeDamage;
    private bool tookDamage = false;
    [SerializeField] private float healthRegenerationDuration;
    private float timer;
    [SerializeField] private TextMeshProUGUI lifeText;


    public void SetDamageBool(bool aBool)
    {
        shouldTakeDamage = aBool;

        if (!shouldTakeDamage)
        {
            lifeText.text = "";
            health = 1;
        }
        else
        {
            health = maxHealth;
            UpdateUI();
        }
    }


    public void UpdatePlayerHealth()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                health += regenarationAmount;
                if(health > maxHealth) health = maxHealth;
                else if(health < maxHealth) timer = healthRegenerationDuration;

                UpdateUI();
            }
        }
    }


    public void TakeDamage(int someDamage)
    {
        if (shouldTakeDamage)
        {
            health -= someDamage;
            UpdateUI();
            timer = healthRegenerationDuration;
        }
    }


    private void UpdateUI() => lifeText.text = health.ToString();


    public bool IsDead()
    {
        if (health > 0) return false;
        else return true;
    }
}

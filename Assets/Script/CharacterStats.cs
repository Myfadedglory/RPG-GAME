using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat strength;
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private double currentHealth;

    private Entity entity;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

        entity = GetComponent<Entity>();
    }

    public virtual void DoDamage(CharacterStats target)
    {
        double totalDamage = damage.GetValue() + strength.GetValue();

        target.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(double damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
    }
}

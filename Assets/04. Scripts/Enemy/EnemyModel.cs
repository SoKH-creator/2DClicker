using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public EnemyData data;
    public int currentHP;

    public event Action OnDamaged;
    public event Action OnDead;

    public EnemyModel(EnemyData data)
    {
        this.data = data;
        currentHP = data.maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        OnDamaged?.Invoke();

        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDead?.Invoke();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTypeA : EnemyBase
{
    void Start()
    {
        base.Start();
    }
    void Update()
    {
        HPUpdate();
        if (m_HP <= 0)
        {
            Destroy(m_HPUI);
            Death();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
    }
}
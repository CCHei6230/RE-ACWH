using System;
using UnityEngine;

public class CubicBullet : WeaponBase
{
    void Start()
    {
        m_speed = 750.0f;
        m_lockOnSpeed = 1.5f;
        m_damage = 25;
    }
    void FixedUpdate()
    {
        if (m_isLockOnAtk)
        {
            LockOnShooting();
        }
        else
        {
            Shooting();
        }
    }
    protected override void  LockOnShooting()
    {
        if (m_LifeTime > 3.0f)
        {
            Destroy(gameObject);
        }
        m_LifeTime += 5.0f * Time.fixedDeltaTime;
        if (m_Target)
        {
            m_TargetPosition = m_Target.transform.position;
        }
        transform.position = Vector2.LerpUnclamped(m_StartPosition, m_TargetPosition, m_LifeTime);
    }
     //------------------------------------------------------------------------------------------------------------
    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Target)
        {
            if (other.transform.root.gameObject == m_Target)
            {
                switch (other.tag)
                {
                    case "Enemy": 
                        var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                        tmp_Score.ScoreIncrease(4);
                        var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                        tmp_enemy.TakeDamage(m_damage*2);
                        if (m_Target)
                        {           
                            Destroy(Instantiate(m_effectPrefab,tmp_enemy.Sprite.transform.position, Quaternion.identity), 2f);
                        }
                        if (tmp_enemy.HP <= 0)
                        {
                            tmp_Score.ScoreIncrease(30);
                            tmp_Score.ComboIncrease();
                            tmp_Score.ExWeaponFinish();
                        }
                        Destroy(transform.gameObject);
                        break;
                }
            }
        }
        else
        {
            switch (other.tag)
            {
                case "Enemy":
                    var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                    tmp_Score.ScoreIncrease(2);
                    var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                    tmp_enemy.TakeDamage(m_damage);
                    if (tmp_enemy.HP <= 0)
                    {
                        tmp_Score.ComboIncrease();
                        tmp_Score.ExWeaponFinish();
                    }
                    Destroy(Instantiate(m_effectPrefab, tmp_enemy.Sprite.transform.position, Quaternion.identity), 2f);
                    Destroy(transform.gameObject);
                    break;
            }
        }
    }
    #endregion
}

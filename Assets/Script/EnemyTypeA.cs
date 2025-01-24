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
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger) { return; }
        switch (other.tag)
        {
            /*
            case  "Player":
                var tmp_player =  other.transform.root.GetComponent<PlayerStatus>();
                tmp_player.TakeDamage(m_Damage);
                print("Player is taking damage from " + name);
                print("Player's HP :" + tmp_player.HP);
                break;
                */
            case "PlayerAtk":
                break;
        }
    }
}
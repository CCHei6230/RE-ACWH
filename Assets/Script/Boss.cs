using System;
using CustomEnum;
using UnityEngine;

public class Boss  : EnemyBase
{
    [SerializeField]  GameObject m_spawnEffPrefab;
    [SerializeField]  GameObject m_SPSkillPrefab;
    public  Transform AtkPos;
    [SerializeField]  Animator m_Anim;    
    [SerializeField]  PlayerMovement m_Player;
    void Start()
    {
        Destroy(Instantiate(m_spawnEffPrefab,transform.position,Quaternion.identity),1f);
        base.Start();
        m_HPMax = 7500;
        m_HP = m_HPMax;
        m_Player = FindFirstObjectByType<PlayerMovement>();
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
    void Anim_Attack(GameObject _AtkPrefab)
    {
        Instantiate(_AtkPrefab,AtkPos.position,Quaternion.identity);
    }
    void Anim_SPSkill(GameObject _AtkPrefab)
    {
        Instantiate(m_SPSkillPrefab,GameObject.Find("Overlayer").transform);
    }
    void Anim_AttackSP(GameObject _AtkPrefab)
    {
        Instantiate(_AtkPrefab,m_Player.transform.position,Quaternion.identity);
    }
    void Anim_SetFacing(Facing _facing)
    {
        m_facing = _facing;
    }
}
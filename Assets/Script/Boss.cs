using System;
using CustomEnum;
using UnityEngine;
using Random = UnityEngine.Random;

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
        m_HPMax = 2500;
        m_HP = m_HPMax;
        m_Player = FindFirstObjectByType<PlayerMovement>();
    }
    void Update()
    {
        HPUpdate();
        m_Anim.SetInteger("HP",m_HP);
    }
    void Anim_RandomAnim()
    {
        int animCurrent = m_Anim.GetInteger("AtkAnim");
        Debug.Log(animCurrent);
        int tmp_rand = Random.Range(0, 3);
        while (tmp_rand == animCurrent)
        {
            tmp_rand = Random.Range(0, 3);
        }
        m_Anim.SetInteger("AtkAnim",tmp_rand);
        Debug.Log(tmp_rand);

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
    void Anim_Death()
    {
        var tmp_ToResult = FindAnyObjectByType<ToNextPointOrToResult>();
        tmp_ToResult.ToResult=true;
        tmp_ToResult.FadeOut();
        Destroy(m_HPUI);
        Death();
    }
    void Anim_SetFacing(Facing _facing)
    {
        m_facing = _facing;
    }
}
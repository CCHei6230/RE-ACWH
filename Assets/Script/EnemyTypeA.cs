using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomEnum ;


public class EnemyTypeA : EnemyBase
{
    [SerializeField] BoxCollider2D m_physicCollider;
    [SerializeField] LayerMask whichIsGround;
    [SerializeField] int m_walkCountMax = 240;
    [SerializeField] int m_walkCount = 0;
    [SerializeField]  GameObject m_AtkPrefab;
    [SerializeField]  Animator m_Anim;    
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
        if (GroundCheck())
        {
            if (CoroutineWalk == null)
            {
                m_walkCount = 0;
                CoroutineWalk = IEnumerator_Walk();
                StartCoroutine(CoroutineWalk);
            }
        }
    }
    bool GroundCheck()
    {
        Vector2 tmp_posL = new Vector2(m_physicCollider.bounds.min.x, m_physicCollider.bounds.min.y);
        Vector2 tmp_posR = new Vector2(m_physicCollider.bounds.max.x, m_physicCollider.bounds.min.y);
        var tmp_rayL = Physics2D.Raycast(tmp_posL, Vector2.down, 0.1f, whichIsGround);
        var tmp_rayR = Physics2D.Raycast(tmp_posR, Vector2.down, 0.1f, whichIsGround);
        if (tmp_rayL)
        {
            return true;// (m_groundPos.position.y > tmp_rayL.collider.bounds.max.y);
        }
        if (tmp_rayR)
        {
            return true;//(m_groundPos.position.y > tmp_rayR.collider.bounds.max.y);
        }
        return  false;
    }
    bool WallCheck()
    {
        Vector2 tmp_pos= Vector2.zero;
        switch (m_facing)
        {
            case Facing.L:
                tmp_pos = new Vector2(m_physicCollider.bounds.min.x,m_physicCollider.bounds.max.y);
                break;
            case Facing.R:
                tmp_pos = new Vector2(m_physicCollider.bounds.max.x, m_physicCollider.bounds.max.y);
                break;
        }
        var tmp_ray = Physics2D.Raycast(tmp_pos, Vector2.right * (int)m_facing, 0.15f, whichIsGround);
        return tmp_ray;
    }

    IEnumerator CoroutineWalk = null;
    IEnumerator IEnumerator_Walk()
    {
        while (m_walkCount < m_walkCountMax )
        {
            m_rb.MovePosition(transform.position + new Vector3(1.5f * (int)m_facing * Time.deltaTime, 0));
            m_walkCount++;
            if(WallCheck())
            {break;}
            yield return new WaitForFixedUpdate();
        }

        if (m_facing == Facing.R)
        {
            m_facing = Facing.L;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            m_facing = Facing.R;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (m_walkCountMax != 480)
        {
            m_walkCountMax = 480;
        }

        CoroutineWalk = null;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }
}
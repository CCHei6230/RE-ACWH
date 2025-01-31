using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomEnum ;
public class EnemyTypeB : EnemyBase
{
    [SerializeField] BoxCollider2D m_physicCollider;
    [SerializeField] LayerMask whichIsGround;
    [SerializeField] LayerMask whichIsPlayer;
    [SerializeField] int m_walkCountMax = 240;
    [SerializeField] int m_walkCount = 0;
    [SerializeField]  GameObject m_AtkPrefab;
    [SerializeField] bool m_isAtk = false;
    [SerializeField] Transform[] m_ShootPos;
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
        if (CoroutineWalk == null)
        {
            m_walkCount = 0;
            CoroutineWalk = IEnumerator_Walk();
            StartCoroutine(CoroutineWalk);
        }
    }
    bool PlayerCheck()
    {
        var tmp_rayL = Physics2D.Raycast(m_ShootPos[0].position, Vector2.down, 5f, whichIsPlayer);
        var tmp_rayR = Physics2D.Raycast(m_ShootPos[1].position, Vector2.down, 5f, whichIsPlayer);
        if (tmp_rayL)
        {
            return true;// (m_groundPos.position.y > tmp_rayL.collider.bounds.max.y);
        }
        else if (tmp_rayR)
        {
            return true;// (m_groundPos.position.y > tmp_rayL.collider.bounds.max.y);
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
    IEnumerator CoroutineAtk = null;

    IEnumerator IEnumerator_Atk()
    {
        
        m_isAtk = true;
        Destroy(Instantiate(m_AtkPrefab, m_ShootPos[0].position, Quaternion.identity), 3f);
        Destroy(Instantiate(m_AtkPrefab, m_ShootPos[1].position, Quaternion.identity), 3f);
        yield return new WaitForSeconds(4f);
        m_isAtk = false;
        CoroutineAtk = null;
    }

    IEnumerator IEnumerator_Walk()
    {
        while (m_walkCount < m_walkCountMax )
        {
            if (PlayerCheck() && !m_isAtk)
            {
                if (CoroutineAtk == null)
                {
                    CoroutineAtk = IEnumerator_Atk();
                    StartCoroutine( CoroutineAtk );
                    yield return new WaitForSeconds(1);
                }
            }

            m_rb.MovePosition(transform.position + new Vector3(1.5f * (int)m_facing , Mathf.Sin(Time.time*4f)*2f )* Time.deltaTime);
            m_walkCount++;
            if(WallCheck())
            {break;}
            yield return new WaitForFixedUpdate();
        }

        if (m_facing == Facing.R)
        {
            m_facing = Facing.L;
        }
        else
        {
            m_facing = Facing.R;
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
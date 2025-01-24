using UnityEngine;
using CustomEnum;


public class Bullet : WeaponBase
{
    #region Member
    [SerializeField] TrailRenderer m_Trail;
    [SerializeField] AnimationCurve m_NormalCurve;
    [SerializeField] GameObject m_Effect2Prefab;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
        m_speed = 2000.0f;
        m_lockOnSpeed = 5.0f;
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
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    override public void  Initialize(Facing _facing,Vector2 _StartPosition)
    {
        m_Trail.widthCurve = m_NormalCurve;
        m_facing = _facing;
        transform.localScale = new Vector3((int)m_facing, 1, 1);
        m_StartPosition = _StartPosition;
        m_damage = 1;
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Target)
        {
            if (!other.isTrigger || m_LifeTime < 0.75f) { return; }
            if (other.transform.root.gameObject == m_Target)
            {
                switch (other.tag)
                {
                    case "Enemy":
                        var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                        tmp_Score.ScoreIncrease(2);
                        var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                        tmp_enemy.TakeDamage(m_damage*2);
                        if (m_Target)
                        {           
                            Destroy(Instantiate(m_effectPrefab, other.transform.position, Quaternion.identity), 2f);
                        }
                        if (tmp_enemy.HP <= 0)
                        {
                            tmp_Score.ScoreIncrease(10);
                            tmp_Score.ComboIncrease();
                        }
                        Destroy(this);
                        Destroy(m_rigidbody);
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
                    tmp_Score.ScoreIncrease(1);
                    var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                    tmp_enemy.TakeDamage(m_damage);
                    Destroy(Instantiate(m_Effect2Prefab, transform.position, Quaternion.identity), 2f);
                    if (tmp_enemy.HP <= 0)
                    {
                        tmp_Score.ComboIncrease();
                    }
                    Destroy(this);
                    Destroy(m_rigidbody);
                    break;
                case "Ground":
                    Destroy(Instantiate(m_Effect2Prefab, transform.position, Quaternion.identity), 2f);
                    Destroy(this);
                    Destroy(m_rigidbody);
                    break;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!m_Target)
        {
            if(other.CompareTag("Ground"))
            {
                Destroy(Instantiate(m_Effect2Prefab, transform.position, Quaternion.identity), 2f);
                Destroy(this);
                Destroy(m_rigidbody);
            }
        }
    }
    #endregion
}
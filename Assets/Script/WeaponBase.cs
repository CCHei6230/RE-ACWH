using UnityEngine;
using CustomEnum;
using CustomInterfaces;
using UnityEngine.Serialization;

public abstract class WeaponBase : MonoBehaviour 
{
    #region Member
    [SerializeField]public int m_damage = 5;
    [SerializeField]protected float m_speed = 2000.0f ;
    [SerializeField]protected float m_lockOnSpeed ;
    [SerializeField]protected Facing m_facing = Facing.R;
    [SerializeField]protected GameObject m_effectPrefab;
    [SerializeField]protected SpriteRenderer m_spriteRenderer;
    [SerializeField]protected Collider2D m_collider;
    [SerializeField]protected Rigidbody2D m_rigidbody;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("LockOnAtk")]
    [SerializeField]protected float m_LifeTime = 0f;
    [SerializeField]protected bool m_isLockOnAtk = false;
    protected Vector2 m_StartPosition;
    protected Vector2 m_CenterPosition;
    protected Vector2 m_TargetPosition;
    [SerializeField]protected GameObject m_Target;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Normal
    public virtual void  Initialize(Facing _facing,Vector2 _StartPosition)
    {
        m_facing = _facing;
        transform.localScale = new Vector3((int)m_facing, 1, 1);
        m_StartPosition = _StartPosition;
        m_damage = 1;
    }
    protected virtual void  Shooting()
    {
        if (!m_spriteRenderer.isVisible)
        {
            Destroy(m_collider);
            Destroy(gameObject,3.0f);
        }
        m_rigidbody.linearVelocity = new Vector2((int)m_facing*m_speed*Time.smoothDeltaTime,0);
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region LockOn
    public virtual void  InitializeLockOn(Facing _facing,Vector2 _StartPosition,GameObject _Target,Vector2 _TargetPosition)
    {
        m_facing = _facing;
        transform.localScale = new Vector3((int)m_facing, 1, 1);
        if (!_Target) { return; }
        m_isLockOnAtk = true;
        m_Target = _Target;
        m_StartPosition = _StartPosition;
        m_CenterPosition = (Vector2)m_Target.transform.position + new Vector2(Random.Range(-10f,10f), Random.Range(-10f,10f));
        m_damage = 5;
    }
    protected virtual void  LockOnShooting()
    {
        if (m_LifeTime > 1.0f && m_Target )
        {
            Destroy(this);
        }
        if (m_LifeTime > 3.0f)
        {
            Destroy(gameObject);
        }
        m_LifeTime += m_lockOnSpeed * Time.fixedDeltaTime;
        if (m_Target)
        {
            m_TargetPosition = m_Target.transform.position;
        }
        transform.position = bezierCurveVector2();
    }
    protected virtual Vector2 bezierCurveVector2()
    {
        Vector2 tmp_AtoB = Vector2.Lerp( m_StartPosition,m_CenterPosition,m_LifeTime);
        Vector2 tmp_BtoC = Vector2.Lerp(m_CenterPosition,m_TargetPosition,m_LifeTime);
        return   Vector2.LerpUnclamped(tmp_AtoB, tmp_BtoC, m_LifeTime);
    }
    #endregion
}
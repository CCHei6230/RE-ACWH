using CustomEnum;
using UnityEngine;

public class Cutter : WeaponBase
{
    #region Member
    [SerializeField]bool m_charged = false;
    [SerializeField]bool m_shoot = false;
    [SerializeField]bool m_collision = false;
    public Transform ShootPosition;
    GameObject Target;
    public  Animator Animator;
    PlayerMovement Player;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
        m_damage = 4;
        Player = FindAnyObjectByType<PlayerMovement>();
    }
    void Update()
    {
        if (!m_charged)
        {
            m_facing = Player.Facing;
            if (m_facing == Facing.L)
            {
                transform.position = ShootPosition.position - new Vector3(0.5f, 0, 0);
            }
            else
            {
                transform.position = ShootPosition.position + new Vector3(0.5f, 0, 0);
            }
        }
        else
        {
            if (!m_shoot)
            {
                m_facing = Player.Facing;
                 if (m_facing == Facing.L)
                {
                    transform.position = ShootPosition.position - new Vector3(0.5f, 0, 0);
                }
                else
                {
                    transform.position = ShootPosition.position + new Vector3(0.5f, 0, 0);
                } 
            }
            else
            {
                if (!m_collision)
                {
                    if (Target)
                    {
                        transform.position = Vector3.MoveTowards
                            (transform.position, Target.transform.position, 17.5f * Time.deltaTime);
                    }
                    else
                    {
                        if (m_facing == Facing.L)
                        {
                            transform.position -= new Vector3(15f, 0, 0) * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(15f, 0, 0) * Time.deltaTime;
                        }
                    }
                }
            }
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Collision
    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                tmp_Score.ScoreIncrease(1);
                var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                Destroy(Instantiate(m_effectPrefab, other.transform.position, Quaternion.identity), 2f);
                tmp_enemy.TakeDamage(m_damage);
                m_collision = true;
                if (tmp_enemy.HP <= 0)
                {
                    tmp_Score.ComboIncrease();
                    tmp_Score.ExtraFinish(false);
                }
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                tmp_Score.ScoreIncrease(1);
                var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                Destroy(Instantiate(m_effectPrefab, other.transform.position, Quaternion.identity), 2f);
                tmp_enemy.TakeDamage(m_damage);
                m_collision = true;
                if (tmp_enemy.HP <= 0)
                {
                    tmp_Score.ComboIncrease();
                    tmp_Score.ExtraFinish(false);
                }
                break;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                m_collision = false;
                break;
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Animation
    void Anim_DestoryGameObj()
    {
        Destroy(gameObject);
    }
    void Anim_SetCharge()
    {
        m_charged = true;
    }
    void Anim_SetShoot()
    {
        m_shoot = true;
        Target = FindAnyObjectByType<PlayerLockOnSystem>().TargetSlot;
        if (Target)
        {
            if (Target.transform.position.x > transform.position.x)
            {
                m_facing = Facing.R;
            }
            else
            {
                m_facing = Facing.L;
            }
        }
        else
        {
            m_facing = Player.Facing;
        }
    }
    #endregion
}

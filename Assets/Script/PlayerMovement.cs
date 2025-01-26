using System;
using CustomEnum;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Serialization;
public class PlayerMovement : MonoBehaviour
{
    #region Member
    [SerializeField]PlayerInput m_playerInput = null;
    [SerializeField]SpriteRenderer m_SpriteRenderer;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Movement")]
    [SerializeField]float m_speed = 300.0f;
    [SerializeField]float m_xVelocity;
    public float XVelocity { get => m_xVelocity; }
    [SerializeField]float m_yVelocity;
    [SerializeField]Facing m_facing = Facing.R;
    public Facing Facing
    {
        get => m_facing;
    } 
    [SerializeField]Rigidbody2D m_rigidbody; 
    [SerializeField]Collider2D m_physicCollider;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Jump")]
    [SerializeField]int m_jumpCount = 0;
    public int JumpCount { get => m_jumpCount; }
    [SerializeField]int m_jumpCountMax = 15;
    [SerializeField]float m_jumpForce = 300.0f;
    [SerializeField]bool m_isDoubleJump  = false;
    [SerializeField]bool m_isGrounded;
    public bool IsGrounded { get => m_isGrounded; }
    public bool WasGrounded = false;
    [SerializeField]Transform m_groundPos;
    [SerializeField]LayerMask whichIsGround;
    IEnumerator CoroutineJump = null;
    [SerializeField]GameObject m_doubleJumpEffectPrefab;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Dash")]
    [SerializeField]int m_dashCount = -1;
    public bool Dashing { get => m_dashCount != -1; }
    [SerializeField]int m_dashCountMax = 20;
    [SerializeField]bool m_canDash = true;
    [SerializeField]bool m_isAirDashing = false;
    [SerializeField]GhostEffect m_dashEffect;
    IEnumerator CoroutineDash = null;
    [SerializeField]GameObject m_dashEffect2Prefab;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("AirFlip")]
    [SerializeField]int m_airFlipCount = -1;  
    public bool AirFliping  { get => m_airFlipCount != -1; }
    [SerializeField]bool m_airFlipOver = false;  
    [SerializeField]int m_airFlipCountMax = 20;  
    [SerializeField]int m_backYVeclocity =0;  
    [SerializeField]Collider2D m_lockOnCollider;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Damage")]
    [SerializeField]int m_damageCount = -1;
    [SerializeField]int m_damageCountMax = 25;
    public bool Damageing
    {
        get { return m_damageCount != -1; }
    }
    [SerializeField]Collider2D m_damageCollider;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start
    void Start()
    {
        m_lockOnCollider.enabled = false;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerInput = GetComponent<PlayerInput>();
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Movement
    public void MovementUpdate()
    {
        //Damage
        if (m_damageCount != -1)
        {
            m_rigidbody.MovePosition(m_rigidbody.position +
                                     new Vector2(m_speed * 0.01f * -(float)m_facing, m_backYVeclocity) *
                                     Time.fixedDeltaTime);
        }
        else
        {
            if (m_airFlipCount == -1)
            {
                //While Dash
                if (m_dashCount != -1)
                {
                    //While Air Dash or m_XVelocity != 0
                    if (m_isAirDashing || m_xVelocity ==0)
                    {
                        m_rigidbody.linearVelocity = new Vector2((int)m_facing * m_speed, m_yVelocity) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        m_rigidbody.linearVelocity = new Vector2(m_xVelocity*m_speed, m_yVelocity)*Time.fixedDeltaTime;
                    }
                }
                //normal
                else
                {
                    m_rigidbody.linearVelocity = new Vector2(m_xVelocity*m_speed, m_yVelocity)*Time.fixedDeltaTime;
                }
            }
            //While AirFlip
            else
            {
                if (!m_airFlipOver)
                {
                    m_rigidbody.MovePosition(m_rigidbody.position +
                                             new Vector2(m_speed * 0.02f * -(float)m_facing, m_backYVeclocity) *
                                             Time.fixedDeltaTime);
                }
                else
                {
                    m_rigidbody.MovePosition(m_rigidbody.position +
                                             new Vector2(m_speed * 0.02f * -(float)m_facing * 1.5f, m_backYVeclocity) *
                                             Time.fixedDeltaTime);
                }
            }
        }
     
    }
    public void FaceCheck()
    {
        if(!m_isAirDashing && m_airFlipCount == -1 &&  m_damageCount == -1)
            switch (m_xVelocity)
            {
                case 1:
                    m_facing = Facing.R;
                    break;
                case -1:
                    m_facing = Facing.L;
                    break;
            }

        transform.localScale = new Vector3((int)m_facing, 1, 1);
    }
    public void InputAction_XVeclocity(InputAction.CallbackContext context)
    {
        m_xVelocity = Mathf.RoundToInt(context.ReadValue<float>());
    }
    public void InputAction_YVeclocity(InputAction.CallbackContext context)
    {
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Jump
    public void Jump()
    {
        //Start GroundCheck while Falling Down
        if (m_jumpCount <= 0)
        {
            m_isGrounded = GroundCheck();
        }
        //Grounded
        if (m_isGrounded)
        {
            m_isDoubleJump = false;
            m_canDash = true;
            m_jumpCount = 0;
            m_yVelocity = 0;
            if (m_playerInput.actions["Jump"].WasPressedThisFrame())
            {
                m_isGrounded = false;
                m_jumpCount = m_jumpCountMax;
            }
        }
        //Floating
        if (!m_isGrounded)
        {
            //Start Jump
            if (CoroutineJump == null)
            {
                CoroutineJump = IEnumerator_Jump();
                StartCoroutine(CoroutineJump);
            }

            //WallKick 
            if (WallCheck()&& m_playerInput.actions["Jump"].WasPressedThisFrame()
                           && m_xVelocity !=0
                           && m_jumpCount < m_jumpCountMax-5
                           && CoroutineJump != null )
            {
                m_rigidbody.MovePosition(m_rigidbody.position 
                                         + new Vector2(-(float)m_facing * 30.0f,0) * Time.fixedDeltaTime);
                StopCoroutine(CoroutineJump);
                CoroutineJump = null;
                m_jumpCount = m_jumpCountMax;
                if (CoroutineDash != null)
                {
                    StopCoroutine(CoroutineDash);
                    CoroutineDash = null;
                    m_isAirDashing = false;
                    m_lockOnCollider.enabled = false;
                    m_speed = 300.0f;
                    m_dashCount = -1;
                }
                m_canDash = true;
                m_isDoubleJump = false;
            }

            //Double Jump
            //Stop First Jump and Dash 
            if (!m_isDoubleJump && m_playerInput.actions["Jump"].WasPressedThisFrame() 
                                && CoroutineJump != null && m_jumpCount < m_jumpCountMax-5
                                &&m_airFlipCount == -1 && m_damageCount == -1)
            {
                StopCoroutine(CoroutineJump);
                CoroutineJump = null;
                m_isDoubleJump = true;
                m_jumpCount = m_jumpCountMax;
                if (CoroutineDash != null)
                {
                    StopCoroutine(CoroutineDash);
                    CoroutineDash = null;
                    m_isAirDashing = false;
                    m_lockOnCollider.enabled = false;
                    m_speed = 300.0f;
                    m_dashCount = -1;
                }
                Destroy(Instantiate(m_doubleJumpEffectPrefab, transform.position, Quaternion.identity), 1f);
            }
            
            if (m_jumpCount != 0)
            {
                //Falling velocity  greater than  Rising velocity
                if (m_jumpCount > 0)
                {
                    m_yVelocity = (float)m_jumpCount / (float)m_jumpCountMax * m_jumpForce;
                    //Stop First Jump
                    if (m_playerInput.actions["Jump"].WasReleasedThisFrame() && !m_isDoubleJump) 
                    {
                        m_jumpCount = 0;
                        m_yVelocity = 0;
                    }
                }
                else
                {
                    m_yVelocity = (float)m_jumpCount / (float)m_jumpCountMax * m_jumpForce * 1.25f;
                }
                m_rigidbody.gravityScale = 8;
            }
            else
            {
                m_yVelocity = 0;
                m_rigidbody.gravityScale = 0;
            }
        }
    }
    bool GroundCheck()
    {
        Vector3 tmp_posL = new Vector3(m_physicCollider.bounds.min.x, m_groundPos.position.y, m_groundPos.position.z);
        Vector3 tmp_posR = new Vector3(m_physicCollider.bounds.max.x, m_groundPos.position.y, m_groundPos.position.z);
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
    IEnumerator IEnumerator_Jump()
    {
        while (!m_isGrounded && m_jumpCount > -30)
        {
            m_jumpCount--;
            yield return new WaitForFixedUpdate();
        }
        CoroutineJump = null;
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Dash
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
    bool FootWallCheck()
    {
        Vector2 tmp_pos= Vector2.zero;
        switch (m_facing)
        {
            case Facing.L:
                tmp_pos = new Vector2(m_physicCollider.bounds.min.x,m_physicCollider.bounds.min.y);
                break;
            case Facing.R:
                tmp_pos = new Vector2(m_physicCollider.bounds.max.x, m_physicCollider.bounds.min.y);
                break;
        }
        return  Physics2D.Raycast(tmp_pos, Vector2.right * (int)m_facing, 0.15f, whichIsGround);
    }
    IEnumerator IEnumerator_Dash()
    {
        m_lockOnCollider.enabled = true;
        m_dashCount = 0;
        m_speed = 750.0f;
        WasGrounded = m_isGrounded;
        if (WasGrounded)
        {
            var tmp_JumpedAfterDash = false;
            while ((m_dashCount < m_dashCountMax))
            {
                if (m_dashCount == -1 || WallCheck() || m_damageCount != -1)
                {
                    break;
                }
                m_dashCount++;
                if (!m_isGrounded)
                {
                    m_canDash = false;
                    if (m_jumpCount > 0)
                    {
                        m_dashCount = m_dashCountMax - 2;
                        tmp_JumpedAfterDash = true;
                        m_lockOnCollider.enabled = false;
                    }
                    else
                    {
                        if (!tmp_JumpedAfterDash)
                        {
                            break;
                        }
                        else
                        {
                            m_dashCount = m_dashCountMax - 2;
                        }
                    }
                }
                if (m_dashCount % 2 == 0)
                {
                    StartCoroutine(m_dashEffect.SpawnEffect(m_SpriteRenderer.transform));
                }
                yield return  new WaitForFixedUpdate();
                if (m_playerInput.actions["Dash"].WasPressedThisFrame())
                {
                    break;
                }
            }
        }
        else
        {
            m_isAirDashing = true;
            m_canDash = false;
            while ((m_dashCount < m_dashCountMax) )
            {
                //Foot Wall Check
                if (m_dashCount == -1 ||WallCheck() || FootWallCheck() || m_damageCount != -1)
                {
                    break;
                }
                m_jumpCount = 0;
                m_dashCount++;
                if (m_dashCount % 2 == 0)
                {
                    StartCoroutine(m_dashEffect.SpawnEffect(m_SpriteRenderer.transform));
                }
                yield return  new WaitForFixedUpdate();
            }
            m_isAirDashing = false;
        }
        m_lockOnCollider.enabled = false;
        m_speed = 300.0f;
        m_dashCount = -1;
        CoroutineDash = null;
        
    }
    public void InputAction_Dash(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if (CoroutineDash != null && !m_canDash)
            {
                m_isAirDashing = false;
                m_lockOnCollider.enabled = false;
                m_speed = 300.0f;
                m_dashCount = -1;
                StopCoroutine(CoroutineDash);
                CoroutineDash = null;
            }
            if(CoroutineDash == null  && m_canDash)
            {
                CoroutineDash = IEnumerator_Dash();
                StartCoroutine(CoroutineDash);
                Destroy(Instantiate(m_dashEffect2Prefab, transform.position, Quaternion.identity), 1f);
            }
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region AirFlip
    public void AirFlip(float _TargetYposition)
    {
        m_dashCount = m_dashCountMax - 2;
        m_jumpCount = 0;
        if (transform.position.y > _TargetYposition + 0.8f)
        {
            var tmp_XVeclocity = m_playerInput.actions["XVeclocity"].ReadValue<float>();
            if ((tmp_XVeclocity < 0 && m_facing == Facing.L )|| (tmp_XVeclocity > 0 && m_facing == Facing.R))
            {
                m_airFlipOver = true;
            }
            else
            {
                m_airFlipOver = false;
            }
        }
        else
        {
            m_airFlipOver = false;
        }

        StartCoroutine(IEnumerator_AirFlip());
    }
    IEnumerator IEnumerator_AirFlip()
    {
        if (CoroutineDash != null)
        {
            StopCoroutine(CoroutineDash);
            CoroutineDash = null;
        }
        if (CoroutineJump != null)
        {
            StopCoroutine(CoroutineJump);
            CoroutineJump = null;
        }
        if (m_airFlipOver)
                 {
                      if (m_facing == Facing.L)
                             {
                                 m_facing = Facing.R;
                             }
                             else
                             {
                                 m_facing = Facing.L;
                             }
                 }
        m_dashCount = -1;
        m_isAirDashing = false;
        m_jumpCount = 0;
        m_canDash = false;
        m_speed = 300.0f;
        m_lockOnCollider.enabled = false;
        m_airFlipCount = 0;
        m_backYVeclocity = m_airFlipCountMax / 2  + 1;
        while (m_airFlipCount < m_airFlipCountMax)
        {
            if (m_isGrounded && m_airFlipOver)
            {
                break;
            }
            m_airFlipCount++;
            m_backYVeclocity--;
            if (m_airFlipCount % 2 == 0)
            {
                StartCoroutine(m_dashEffect.SpawnEffect(m_SpriteRenderer.transform));
            }
            yield return new WaitForFixedUpdate();
        }
        m_airFlipCount = -1;
        m_backYVeclocity = 0;
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Damage
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_damageCollider.IsTouching(other) && m_damageCount == -1 && m_airFlipCount == -1)// && m_dashCount == -1  )
        {
            switch (other.tag)
            {
                case "Enemy":
                    StartCoroutine(IEnumerator_Damage());
                    var Enemy =   other.transform.root.GetComponent<EnemyBase>();
                    GetComponent<PlayerStatus>().TakeDamage(Enemy.Damage);
                    break;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (m_damageCollider.IsTouching(other) && m_damageCount == -1 && m_airFlipCount == -1  && m_dashCount == -1  )
        {
            switch (other.tag)
            {
                case "Enemy":
                    StartCoroutine(IEnumerator_Damage());
                    var Enemy =   other.transform.root.GetComponent<EnemyBase>();
                    GetComponent<PlayerStatus>().TakeDamage(Enemy.Damage);
                    break;
            }
        }
    }
    IEnumerator IEnumerator_Damage()
    {
        FindAnyObjectByType<InGameManager>().HPAnim();
        m_damageCount = 0;
        m_backYVeclocity = m_damageCountMax / 2  + 1;
        while (m_damageCount < m_damageCountMax)
        {
            m_damageCount++;
            m_backYVeclocity--;
            yield return new WaitForFixedUpdate();
        }
        FindFirstObjectByType<ScoreSystem>().ScoreIncrease(-2);
        m_damageCount = -1;
    }
    #endregion
}

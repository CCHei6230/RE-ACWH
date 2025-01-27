using System;
using CustomEnum;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Serialization;

public class PlayerBehaviour : MonoBehaviour
{
    #region Member
    [SerializeField] PlayerEP m_playerEP;
    [SerializeField] PlayerStatus m_playerStatus;
    [SerializeField] PlayerSpSkill m_playerSpSkill;
    [SerializeField] PlayerWeapons m_playerWeapons;
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] PlayerLockOnSystem m_playerLockOnSystem;
    [SerializeField] PlayerAnimationState m_playerAnimationState;
    [SerializeField] bool m_canControl = true;
    [SerializeField] bool m_died = false;
    [SerializeField] Transform m_shootPosition;
    [SerializeField] Transform m_UIEPPosition;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Updates
    void Start()
    {
        m_playerEP = GetComponent<PlayerEP>();
        m_playerStatus = GetComponent<PlayerStatus>();
        m_playerSpSkill = GetComponent<PlayerSpSkill>();
        m_playerWeapons = GetComponent<PlayerWeapons>();
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAnimationState = GetComponent<PlayerAnimationState>();
        m_playerAnimationState.PlayerMovement = m_playerMovement;
        m_playerSpSkill.Invoke_SPIncrease();
    }
    void FixedUpdate()
    {
        if (m_canControl)
        {
            Shooting();
            ExWeaponAtk();
        }
        else
        {
            m_playerEP.StopEPCoroutine();
            m_playerWeapons.ExAtkCoolDown = -1;
            m_playerWeapons.ShootingCoolDown = -1;
            m_playerWeapons.SetCutterNull();
        }
        m_playerEP.EPUpdate();
        m_playerLockOnSystem.LockOnUpdate();
        m_playerMovement.MovementUpdate();
    }
    void Update()
    {
        m_canControl = !m_playerMovement.Damageing;
        m_playerMovement.Jump();
        m_playerLockOnSystem.CanLockOnTarget = m_playerMovement.Dashing;
    }
    void LateUpdate()
    { 
        if (m_playerStatus.HP <= 0 && !m_died)
        {
            m_died = true;
            var tmp_data = Instantiate(new GameObject()).AddComponent<RestartData>();
            tmp_data.TimerCurrent = FindFirstObjectByType<InGameTimer>().InGameTime;
            tmp_data.SpawnPoint = GetComponent<PlayerSpawn>().SpawnPoint;
            DontDestroyOnLoad(tmp_data);
            
            m_playerStatus.Invoke_Death();
            Destroy(gameObject,0.15f);
            
            var tmp_ToNextPoint = FindAnyObjectByType<TeleportToNextPoint>();
            tmp_ToNextPoint.NextPosition = transform;
            tmp_ToNextPoint.FadeOut();
            FindAnyObjectByType<InGameManager>().Invoke_Restart();
        }
        if (Time.timeScale == 1)
        {
            m_playerMovement.FaceCheck();
            m_playerAnimationState.CurrentState = m_playerAnimationState.NextAnimationState
                (m_playerWeapons.ShootingCoolDown != -1 || m_playerWeapons.ExAtkCoolDown != -1 );
            m_playerAnimationState.Animator.CrossFade(m_playerAnimationState.CurrentState,0,0);
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    void Shooting()
    {
        if (m_playerWeapons.ShootingCoolDown == 0)
        {
            StartCoroutine(
                m_playerWeapons.IEnumerator_Shoot(
                    m_shootPosition,
                    m_playerMovement.Facing,
                    m_playerLockOnSystem.TargetSlot
                )
            );
        }
    }
    void ExWeaponAtk()
    {
        if (m_playerWeapons.ExAtkCoolDown == 0)
        {
            StartCoroutine(
                m_playerWeapons.IEnumerator_ExATK(
                    m_shootPosition,
                    m_playerMovement.Facing,
                    m_playerLockOnSystem.TargetSlot,
                    m_playerEP
                )
            );
            m_playerEP.StopEPCoroutine();
        }

        if (m_playerEP.EP <= 0 )
        {
            m_playerWeapons.SetCutterNull();
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region InputAction
    public void InputAction_SelectExWeapon(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
        if (!context.performed){return;}
        m_playerWeapons.SelectExWeapon(Mathf.RoundToInt(context.ReadValue<float>()));
        if ( m_playerWeapons.ExAtkCoolDown != -1)
        {
            m_playerWeapons.ExAtkCoolDown = 0;
        }
        m_playerWeapons.SetCutterNull();
    }
    public void InputAction_ExWeaponAtk(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
        if (context.started)
        {
            m_playerWeapons.ExAtkCoolDown = 0;
        }
        if (context.canceled)
        {
            m_playerEP.StopEPCoroutine();
            m_playerWeapons.ExAtkCoolDown = -1;
            m_playerWeapons.SetCutterNull();
        }
    }
    public void InputAction_ShootBullet(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
        if (context.canceled)
        {
            m_playerWeapons.ShootingCoolDown = -1;
        }
        if (context.started)
        {
            m_playerWeapons.ShootingCoolDown = 0;
        }
    }
    public void InputAction_Jump(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
    }
    public void InputAction_Dash(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
    }
    public void InputAction_SPSkill(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
        m_playerSpSkill.SPSkill();
    }
    public void InputAction_LockOnDisable(InputAction.CallbackContext context)
    {
        if (!m_canControl) {return;}
        m_playerLockOnSystem.TargetSlot = null;
    }
    #endregion
}
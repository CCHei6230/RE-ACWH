using System;
using System.Collections;
using UnityEngine;
using CustomEnum ;
using UnityEngine.Serialization;

public class PlayerWeapons : MonoBehaviour
{
    #region Member
    [Header("Bullet")]
    [SerializeField] int m_ShootingCoolDown = -1;
    [SerializeField] int m_ShootingCoolDownMax = 12;
    public int ShootingCoolDown
    {
        get => m_ShootingCoolDown;
        set => m_ShootingCoolDown = value;
    }
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_BulletShootEffPrefab;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Ex Weapons")] [SerializeField]
    public int m_ExAtkCoolDown = -1;
    public int ExAtkCoolDown
    {
        get => m_ExAtkCoolDown;
        set => m_ExAtkCoolDown = value;
    }
    [SerializeField] int m_ExAtkCoolDownMax = 10;
    ExWeapons m_ExWeaponSlot = (ExWeapons)0;
    public ExWeapons ExWeaponSlot
    {
        get => m_ExWeaponSlot;
    }
    int m_ExWeaponsAmout = Enum.GetNames(typeof(ExWeapons)).Length;
    [SerializeField] GameObject[] m_ExWeaponsPrefab;
    [SerializeField]public Cutter Cutter;
    [SerializeField]public GameObject CutterObj;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    public void SetCutterNull()
    {
        if (CutterObj)
        {
            Cutter.Animator.SetBool("out",true);
            CutterObj = null;
            Cutter = null;
        }
    }
    public void SelectExWeapon(int _value)
    {
        m_ExWeaponSlot += _value;
        //adjust weapon slot
        if ((int)m_ExWeaponSlot > m_ExWeaponsAmout - 1)
        {
            m_ExWeaponSlot = (CustomEnum.ExWeapons)0;
        }
        else if ((int)m_ExWeaponSlot < 0)
        {
            m_ExWeaponSlot = (CustomEnum.ExWeapons)m_ExWeaponsAmout - 1;
        }
        //set weapon cool down time
        switch (m_ExWeaponSlot)
        {
            case ExWeapons.CubicBullet:
                m_ExAtkCoolDownMax = 25;
                break;
            case ExWeapons.CuttingEdge:
                m_ExAtkCoolDownMax = 2;
                break;
        }
    }
    void Shoot(Transform _shotPosition, Facing _facing, GameObject _target )
    {
        var tmp_WeaponObj =
            Instantiate(m_BulletPrefab, _shotPosition.position, Quaternion.identity)
                .GetComponent<Bullet>();
        if (_target)
        {
            tmp_WeaponObj.InitializeLockOn(_facing,_shotPosition.position,
                _target,_target.transform.position);
        }
        else { tmp_WeaponObj.Initialize(_facing, _shotPosition.position); }
        m_ShootingCoolDownMax = _target ? 8 : 12;
        Destroy(Instantiate(m_BulletShootEffPrefab,_shotPosition),2f);
    }
    void ExAtk(Transform _shotPosition, Facing _facing, GameObject _target ,PlayerEP _playerEP )
    {
        switch (m_ExWeaponSlot)
        {
            case ExWeapons.CubicBullet:
                if (_playerEP.EP >= 10)
                {
                    var tmp_WeaponObj =
                        Instantiate(
                            m_ExWeaponsPrefab[(int)m_ExWeaponSlot], _shotPosition.position, Quaternion.identity
                        ).GetComponent<WeaponBase>();
                    if (_target)
                    {
                        tmp_WeaponObj.InitializeLockOn(_facing, _shotPosition.position, _target, _target.transform.position);
                    }
                    else
                    {
                        tmp_WeaponObj.Initialize(_facing, _shotPosition.position);
                    }
                    _playerEP.EP -= 10;
                    _playerEP.StopEPCoroutine();
                }
                break;
            case ExWeapons.CuttingEdge:
                if (_playerEP.EP >= 1)
                {
                    if (!CutterObj)
                    {
                        CutterObj =
                        Instantiate(m_ExWeaponsPrefab[(int)m_ExWeaponSlot], _shotPosition.position, Quaternion.identity);
                        Cutter =  CutterObj.GetComponent<Cutter>();
                    }
                    Cutter.ShootPosition = _shotPosition;
                    _playerEP.EP -= 1;
                    _playerEP.StopEPCoroutine();
                }
                else
                {
                    if (CutterObj)
                    {
                        Cutter.Animator.SetBool("out",true);
                        CutterObj = null;
                        Cutter =  null;
                    }
                }
                break;
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region IEnumerator
    public IEnumerator IEnumerator_Shoot(Transform _shotPosition , Facing _facing,GameObject _target )
    {
        if (m_ShootingCoolDown == 0) { Shoot(_shotPosition, _facing, _target  ); }
        while (m_ShootingCoolDown < m_ShootingCoolDownMax  && m_ShootingCoolDown != -1) {
            m_ShootingCoolDown++;
            yield return  new WaitForFixedUpdate();
        }
        if (m_ShootingCoolDown >= m_ShootingCoolDownMax) { m_ShootingCoolDown = 0; }
    }
    public IEnumerator IEnumerator_ExATK(Transform _shotPosition , Facing _facing,GameObject _target,PlayerEP _playerEP )
    {
        if (m_ExAtkCoolDown == 0) { ExAtk(_shotPosition, _facing, _target , _playerEP ); }
        while (m_ExAtkCoolDown < m_ExAtkCoolDownMax  && m_ExAtkCoolDown != -1) {
            m_ExAtkCoolDown++;
            yield return  new WaitForFixedUpdate();
        }
        if (m_ExAtkCoolDown >= m_ExAtkCoolDownMax) { m_ExAtkCoolDown = 0; }
    }
    #endregion
}
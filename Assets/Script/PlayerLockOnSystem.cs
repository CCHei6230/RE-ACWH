using UnityEngine;
using System;
using System.Collections;
using CustomInterfaces;
using UnityEngine.Serialization;

public class PlayerLockOnSystem : MonoBehaviour
{
    #region Member
    [SerializeField] int m_LockOnTargetCount = -1;
    [SerializeField] int m_LockOnTargetCounterMax = 240;
    [SerializeField] private bool m_CanLockOnTarget;
    public bool CanLockOnTarget
    {
        get => m_CanLockOnTarget;
        set => m_CanLockOnTarget = value;
    }
    [SerializeField] SpriteRenderer m_TargetSprite;
    [SerializeField] GameObject m_TargetSlot;
    public GameObject TargetSlot
    {
        get => m_TargetSlot;
        set => m_TargetSlot = value;
    }
    [SerializeField] GameObject m_LockOnMark;
    [SerializeField] GameObject m_LockOnMarkPrefab;
    [SerializeField] Collider2D LockOnCollider;
    IEnumerator CoroutineLockOn = null;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    public void LockOnUpdate()
    {
        if (!m_TargetSlot)
        {
            m_TargetSlot = null;
            if (m_LockOnMark)
            {
                Destroy(m_LockOnMark);
                m_LockOnMark = null;
            }
            m_LockOnTargetCount = -1;
            if (CoroutineLockOn != null)
            {
                StopCoroutine(CoroutineLockOn);
                CoroutineLockOn = null;
            }
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region IEnumerator
    public IEnumerator IEnumerator_LockOn()
    {
        m_LockOnTargetCount = 0;
        while (m_LockOnTargetCount < m_LockOnTargetCounterMax  && m_LockOnTargetCount != -1 )
        {
            m_LockOnTargetCount++;
            yield return  new WaitForFixedUpdate();
        }
        if (m_LockOnTargetCount >= m_LockOnTargetCounterMax)
        {
            m_TargetSlot = null;
            Destroy(m_LockOnMark);
            m_LockOnTargetCount = -1;
            CoroutineLockOn = null;
        }
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!m_CanLockOnTarget || !LockOnCollider.IsTouching(other)) return;
        switch (other.tag)
        {
            case "Enemy":
                if (m_LockOnMark)
                {
                    m_TargetSlot = null;
                    Destroy(m_LockOnMark);
                    if (CoroutineLockOn != null)
                    {
                        StopCoroutine(CoroutineLockOn);
                        CoroutineLockOn = null;
                    }
                }
                other.transform.root.GetComponent<iCanBeLockOn>().BeLockOn(out m_TargetSprite , out m_TargetSlot );
                m_LockOnMark = Instantiate(m_LockOnMarkPrefab, GameObject.Find("LockMark").transform);
                var _LockOnMark = m_LockOnMark.GetComponent<UI_ObjFollowTarget>();
                _LockOnMark.Target = m_TargetSprite.transform;
                _LockOnMark.transform.position = Camera.main.WorldToScreenPoint(_LockOnMark.Target.position + _LockOnMark.Offset);
                _LockOnMark.m_fixPosition = true;
                if(CoroutineLockOn == null)
                {
                    CoroutineLockOn = IEnumerator_LockOn();
                    StartCoroutine(CoroutineLockOn);
                }
                GetComponent<PlayerMovement>().AirFlip(other.transform.position.y);
                break;
        }
    }
    #endregion
}
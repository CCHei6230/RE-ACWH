using UnityEngine;
using System;
using System.Collections;
public class PlayerEP : MonoBehaviour
{
    #region Member
    [SerializeField] int m_EP = 100;
    public int EP
    {
        get => m_EP;
        set => m_EP = value;
    }
    [SerializeField]int m_EPMax = 100;
    public int EPMax
    {
        get => m_EPMax;
        set => m_EPMax = value;
    }
    [SerializeField] int m_EPCount = -1;
    [SerializeField] int m_EPCountMax = 35;
    IEnumerator CoroutineEPReloading = null;
    IEnumerator CoroutineEPWaitToReload = null;
    #endregion
    //---------------------------------------------------------------------
    #region Method
    public  void EPUpdate()
    {
        if (m_EP < m_EPMax)
        {
            if(CoroutineEPReloading == null)
                EPReload();
        }

        if (m_EP < 0)
        {
            m_EP = 0;
        }
    }
    public void EPReload()
    {
        if(CoroutineEPWaitToReload == null)
        {
            CoroutineEPWaitToReload = IEnumerator_EPWaitToReload();
            StartCoroutine(CoroutineEPWaitToReload);
        }
    }
    public void StopEPCoroutine()
    {
        if (CoroutineEPWaitToReload != null) {
            StopCoroutine(CoroutineEPWaitToReload);
            CoroutineEPWaitToReload = null;
        }
        if (CoroutineEPReloading != null) {
            StopCoroutine(CoroutineEPReloading);
            CoroutineEPReloading = null;
        }
    }
    #endregion
    //---------------------------------------------------------------------
    #region IEnumerator
    public IEnumerator IEnumerator_EPReloading()
    {
        while (m_EP < m_EPMax   )
        {
            m_EP++;
            yield return  new WaitForFixedUpdate();
        }
        if (m_EP >= m_EPMax)
        {
            m_EP = m_EPMax;
            m_EPCount = -1;
            CoroutineEPWaitToReload = null; 
            CoroutineEPReloading = null;
        }
    }
    public IEnumerator IEnumerator_EPWaitToReload()
    {
        CoroutineEPReloading = null;
        m_EPCount = 0;
        while (m_EPCount < m_EPCountMax  )
        {
            m_EPCount++;
            yield return  new WaitForFixedUpdate();
        }
        if (m_EPCount >= m_EPCountMax)
        {
            m_EPCount = m_EPCountMax;
            if(CoroutineEPReloading == null)
            {
                CoroutineEPReloading = IEnumerator_EPReloading();
                StartCoroutine(CoroutineEPReloading);
            }
        }
    }
    #endregion
}

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class InGameManager : MonoBehaviour
{
    
    #region Member
    
    [SerializeField]SceneObject m_thisScene;
    
    [SerializeField] Image m_UIEP;
    [SerializeField] GameObject m_UIEPPointer;
    [SerializeField] PlayerEP m_PlayerEP;
    [SerializeField] TMP_Text m_EPText;
    
    [SerializeField] Image m_UIHP;
    [SerializeField] PlayerStatus m_PlayerStatus;
    [Header("ExWeapon")]
    [SerializeField] PlayerWeapons m_PlayerWeapons;

    [SerializeField] Sprite[] m_weaponSprite; 
    [SerializeField] Image m_weaponImage; 
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
        m_PlayerWeapons = FindFirstObjectByType<PlayerWeapons>();
    }
    void Update()
    {

        m_weaponImage.sprite = m_weaponSprite[(int)m_PlayerWeapons.ExWeaponSlot];
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(m_thisScene);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            EnemySpawner.ResetSpawner();
        }
        
        float _EPAmout = (float)m_PlayerEP.EP / (float)m_PlayerEP.EPMax;
        m_UIEP.fillAmount = _EPAmout;
        m_UIEPPointer.transform.rotation =
            Quaternion.Euler(m_UIEPPointer.transform.eulerAngles.x,m_UIEPPointer.transform.eulerAngles.y,1- _EPAmout *360.0f);
        m_EPText.text = m_PlayerEP.EP.ToString();
        
        float _HPAmout = (float)m_PlayerStatus.HP / (float)m_PlayerStatus.HPMax;
        m_UIHP.fillAmount = _HPAmout;
    }
    #endregion
}

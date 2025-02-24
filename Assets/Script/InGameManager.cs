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
    [SerializeField] PlayerSpSkill m_PlayerSpSkill;
    [SerializeField] Image m_SPImage; 

    
    
    [SerializeField] Sprite[] m_weaponSprite; 
    [SerializeField] Image m_weaponImage; 
    
    
    
    
    [SerializeField]Animator m_Anim_HP;
    [SerializeField]Animator m_Anim_Damage;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
        m_PlayerWeapons = FindFirstObjectByType<PlayerWeapons>();
        m_PlayerSpSkill = FindFirstObjectByType<PlayerSpSkill>();
    }
    void Update()
    {
        m_SPImage.fillAmount = (float)m_PlayerSpSkill.SP / (float)m_PlayerSpSkill.SPMax;
        m_Anim_Damage.SetInteger("HP",m_PlayerStatus.HP);
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

    void Restart()
    {
        SceneManager.LoadScene(m_thisScene);
    }

    public void HPAnim()
    {
        m_Anim_HP.SetTrigger("In");
        m_Anim_Damage.SetTrigger("In");
    }

    public void Invoke_Restart()
    {
        Invoke("Restart", 1.5f);
    }
    
    #endregion
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    #region Member
    [Header("Score")]
    [SerializeField]int m_score = 0;
    [SerializeField]float m_scorePresent = 0; 
    [SerializeField]TMP_Text m_scoreText;
    [SerializeField]Animator m_scoreAnim;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Combo")]
    [SerializeField]int m_combo = 0;
    [SerializeField]int m_comboBouns = 0;
    [SerializeField]int m_comboCountDown = -1;
    [SerializeField]int m_comboCountDownMax = 240;
    [SerializeField]TMP_Text m_comboText;
    [SerializeField]Image m_comboImage;
    [SerializeField]Animator m_comboAnim;   
    IEnumerator CoroutineCombo = null;
    [Header("------------------------------------------------------------------------------------------------------------")]
    [Header("Extra")]
    [SerializeField]int m_ExtraCountDown = -1;
    [SerializeField]Animator m_ExtraAnim;
    [SerializeField]TMP_Text m_ExtraText;
    IEnumerator CoroutineExWeapon = null;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
    }
    void Update()
    {
        ScoreUpdate();
        ComboUpdate();
        ExweaponBonusUpdate();
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Combo
    void ComboUpdate()
    {
       m_comboAnim.SetBool("In",m_comboCountDown != -1);
       if (m_comboCountDown != -1)
       {
           m_comboImage.fillAmount = 1- (float)m_comboCountDown / (float)m_comboCountDownMax;
       }
       else
       {
           m_comboImage.fillAmount = 0;
       }
       if (m_combo != 0)
       {
           if (m_combo == 1)
           {
               m_comboText.text = m_combo.ToString() + " Chain  ";
           }
           else
           {
               m_comboText.text = m_combo.ToString() + " Chain +" + m_comboBouns;
           }
       }
    }
    public  void ComboIncrease()
    {
        m_combo ++;
        if (CoroutineCombo != null)
        {
            StopCoroutine(CoroutineCombo);
        }

        if (m_combo > 1)
        {
            m_comboBouns = m_combo  * 10;
        }
        if (m_comboBouns > 50)
        {
            m_comboBouns = 50;
        }
        ScoreIncrease(m_comboBouns);
        CoroutineCombo = IEnumerator_Combo();
        StartCoroutine(CoroutineCombo);
    }
    IEnumerator IEnumerator_Combo()
    {
        m_comboCountDown = 0;
        while (m_comboCountDown < m_comboCountDownMax)
        {
            m_comboCountDown++;
            yield return  new WaitForFixedUpdate();
        }
        m_comboBouns = 0;
        m_combo = 0;
        m_comboCountDown = -1;
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region ExweaponBonus
    void ExweaponBonusUpdate()
    {
        m_ExtraAnim.SetBool("In",m_ExtraCountDown != -1);
    }
    public void ExtraFinish(bool _SPSkill)
    {
        if (_SPSkill)
        {
            ScoreIncrease(100);
            m_ExtraText.text = "SP Skill +" + m_combo*100;
        }
        else
        {
            ScoreIncrease(30);
            m_ExtraText.text = "EX-Weapon +30";
        }

        if (CoroutineExWeapon != null)
        {
            StopCoroutine(CoroutineExWeapon);
        }
        CoroutineExWeapon = IEnumerator_ExWeapon();
        StartCoroutine(CoroutineExWeapon);
    }
    IEnumerator IEnumerator_ExWeapon()
    {
        m_ExtraCountDown = 0;
        while (m_ExtraCountDown < 120)
        {
            m_ExtraCountDown++;
            yield return  new WaitForFixedUpdate();
        }
        m_ExtraCountDown = -1;
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Score
    void ScoreUpdate()
    {
        if (m_scorePresent != m_score)
        {
            m_scorePresent = Mathf.Lerp(m_scorePresent, m_score, Time.deltaTime * 10f);
        }
        if (m_score <= 0)
        {
            m_score = 0;
        }
        m_scoreText.text =Mathf.Round(m_scorePresent).ToString();
        m_scoreAnim.SetBool("In",m_score > 0);
    }
    public  void ScoreIncrease(int _score)
    {
        m_score += _score;
    }
    #endregion
}

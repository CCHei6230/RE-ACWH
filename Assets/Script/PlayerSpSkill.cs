using UnityEngine;

public class PlayerSpSkill : MonoBehaviour
{
    #region Member
    [SerializeField] int m_SP= 0;
    [SerializeField] int m_SPMax = 100;
    public int SP
    {
        get => m_SP;
        set => m_SP = value;
    }
    public int SPMax
    {
        get => m_SPMax;
    }
    [SerializeField]GameObject m_SPSKillPrefab;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
        m_SP = 0;
        m_SPMax = 100;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            m_SP = m_SPMax;
        }
    }

    public void Invoke_SPIncrease()
    {
        InvokeRepeating("SPIncrease" , 0 ,1f);
    }
    void SPIncrease()
    {
        m_SP++;
        if (m_SP > m_SPMax)
        {
            m_SP = m_SPMax;
        }
    }
    public void SPSkill()
    {
        if (m_SP < SPMax) { return; }
        m_SP = 0;
        Instantiate(m_SPSKillPrefab,GameObject.Find("SpSkill").transform);
    }
    #endregion
}

using UnityEngine;
using CustomEnum;
using CustomInterfaces;

public class PlayerStatus : MonoBehaviour ,iDamagable
{
    #region Member
    [SerializeField]
    protected int m_HP = 100;
    public int HP
    {
        get  => m_HP; 
        set => m_HP = value; 
    }
    protected int m_HPMax = 100;
    public int HPMax
    {
        get  => m_HPMax; 
        set => m_HPMax = value; 
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Start & Update
    void Start()
    {
    }
    void Update()
    {
    }
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    public void TakeDamage( int _damage) => m_HP-= _damage; 
    #endregion
}
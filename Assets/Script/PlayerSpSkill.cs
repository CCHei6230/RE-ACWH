using UnityEngine;

public class PlayerSpSkill : MonoBehaviour
{
    #region Member
    [SerializeField] int m_SP= 0;
    public int SP
    {
        get => m_SP;
        set => m_SP = value;
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
}

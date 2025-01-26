using UnityEngine;
using CustomEnum;
using CustomInterfaces;

public class PlayerStatus : MonoBehaviour ,iDamagable
{
    #region Member
    [SerializeField] private GameObject m_deathEffectPrefab;
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
    #endregion
    //------------------------------------------------------------------------------------------------------------
    #region Method
    public void TakeDamage( int _damage) => m_HP-= _damage;
    public void Invoke_Death()
    {
        Invoke("Death", 0.05f);
    }
    public void Death()
    {
        var tmp_deathEff =  Instantiate(m_deathEffectPrefab, transform.position, Quaternion.identity);
        tmp_deathEff.GetComponent<DeathSprite>().SetSprite(GetComponentInChildren<SpriteRenderer>());
        Destroy(tmp_deathEff,1f);
        //Destroy(gameObject);
    }

    #endregion
}
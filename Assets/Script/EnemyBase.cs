using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomEnum ;
using UnityEngine.Serialization;
using CustomInterfaces;
using Unity.Mathematics;

public abstract class EnemyBase : MonoBehaviour , iDamagable,iCanBeLockOn
{
    protected Rigidbody2D m_rb;
    [SerializeField] private GameObject m_deathEffectPrefab;
    [SerializeField]protected int m_HP = 100;
    protected int m_HPMax = 100;
    public int HP
    {
        get  => m_HP; 
        set => m_HP = value; 
    }
    [SerializeField]protected int m_Damage;
    public int Damage
    {
        get  => m_Damage; 
        set => m_Damage = value; 
    }
    [SerializeField]protected SpriteRenderer m_Sprite;
    public SpriteRenderer Sprite { get => m_Sprite; }
    
    [SerializeField] protected Material m_SpriteMaterial;
    [SerializeField] protected Material m_SpriteFlashMaterial;

    protected IEnumerator CoroutineDamageFlash= null;
    [SerializeField]protected GameObject m_HPUIPrefab;
    [SerializeField]protected GameObject m_HPUI;
    [SerializeField]protected Image m_HPUIImage;
    [SerializeField]protected Facing m_facing = Facing.L;
    protected void Start()
    {
        m_HP = 100;
        m_HPUI = Instantiate
            (m_HPUIPrefab, GameObject.Find("UI_EnemyHP").transform);
        m_HPUIImage = m_HPUI.transform.GetChild(0).GetComponent<Image>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator IEnumerator_DamageFlash()
    {
        var tmp_count = 0;
        
        while (tmp_count < 4  ) {
            tmp_count++;
            if (tmp_count % 2 == 0)
            {
                m_Sprite.material = m_SpriteFlashMaterial;
                m_Sprite.transform.localScale = Vector3.one*1.15f*3f;
            }
            else
            {
                m_Sprite.material = m_SpriteMaterial;
                m_Sprite.transform.localScale  = Vector3.one*3f;
            }
            yield return  new WaitForFixedUpdate();
        }
        if (tmp_count >= 4)
        {
            m_Sprite.material = m_SpriteMaterial;
            m_Sprite.transform.localScale  = Vector3.one*3f;
            CoroutineDamageFlash=null;
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        m_HP-=_damage;
        if (CoroutineDamageFlash == null)
        {
            CoroutineDamageFlash = IEnumerator_DamageFlash();
            StartCoroutine(CoroutineDamageFlash);
        }
    }
    public void BeLockOn(out SpriteRenderer _Sprite ,out GameObject _RootObj)
    {
        _Sprite = m_Sprite;
        _RootObj = transform.root.gameObject;
    }

    protected void HPUpdate()
    {
        m_HPUI.transform.position = 
            Camera.main.WorldToScreenPoint(Sprite.transform.position + new Vector3(0,1.5f,0));
        m_HPUIImage.fillAmount = (float)m_HP / (float)m_HPMax;
        m_HPUIImage.color = Color.Lerp(Color.red,Color.yellow, m_HPUIImage.fillAmount);
    }
    public virtual void Death()
    {
       var tmp_deathEff =  Instantiate(m_deathEffectPrefab, m_Sprite.transform.position, Quaternion.identity);
       tmp_deathEff.GetComponent<DeathSprite>().SetSprite(m_Sprite);
       Destroy(tmp_deathEff,1f);
       Destroy(gameObject);
    }
}
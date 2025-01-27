using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostEffect : MonoBehaviour
{
    #region Member
    [SerializeField] Color m_color;
    [SerializeField] int m_lifeTimeMax;
    [SerializeField] Material m_material;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    #endregion
    //------------------------------------------------------------------------------------------------------------
    public IEnumerator SpawnEffect(Transform _transform  )
    {
        var tmp_effObj = new GameObject(_transform.root.name + "GhostEffect");
        tmp_effObj .transform.position = _transform.position;
        tmp_effObj.transform.localScale = _transform.lossyScale;
        var tmp_effSR = tmp_effObj.AddComponent<SpriteRenderer>();
        tmp_effSR.sprite = m_spriteRenderer.sprite;
        tmp_effSR.material = m_material;
        float tmp_colorA ;
        int tmp_LifeTime = 0;
        while (tmp_LifeTime < m_lifeTimeMax)
        {
            tmp_LifeTime++;
            if (tmp_LifeTime >1)
            {
                tmp_colorA = (1.0f - (float)tmp_LifeTime / (float)m_lifeTimeMax) * m_color.a;

            }
            else
            {
                tmp_colorA = 0;
            }

            tmp_effSR.color = new Color(m_color.r,m_color.g,m_color.b,tmp_colorA);
            yield return new WaitForFixedUpdate();
        }
        Destroy(tmp_effObj);
    }
}

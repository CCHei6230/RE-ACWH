using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UI_EP : UI_ObjFollowTarget
{
    PlayerEP m_playerEp;
    [SerializeField] TMP_Text[] m_EPTexts;
    [SerializeField] Image[] m_EPImages;
    IEnumerator CoroutineFade = null;  
    [SerializeField]bool m_fadeDone = true;
    void Start()
    {
        m_playerEp = FindFirstObjectByType<PlayerEP>();
        m_cam = Camera.main;
    }
    void Update()
    {
        if (m_playerEp.EP == m_playerEp.EPMax)
        {
            if (CoroutineFade == null && !m_fadeDone)
            {
                CoroutineFade =  IEnumerator_Fade(false);
                StartCoroutine(CoroutineFade);
            }
        }
        else
        {
            if (CoroutineFade == null && m_fadeDone)
            {
                CoroutineFade =  IEnumerator_Fade(true);
                StartCoroutine(CoroutineFade);
            }
        }
        if (!Target) return;
        m_posOnScreen = m_cam.WorldToScreenPoint(Target.position + Offset);
        if (transform.position == m_posOnScreen) return;
        if (m_fixPosition)
        {
            transform.position = m_posOnScreen;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_posOnScreen, Time.deltaTime*6f);
        }
    }
    IEnumerator IEnumerator_Fade(bool _fadeIn)
    {
        int tmp_count = 0;
        float tmp_alpha = 0f;
        while (tmp_count  < 5)
        {
            tmp_count++;
            if (_fadeIn)
            {
                tmp_alpha = (float)tmp_count / 5.0f;
            }
            else
            {
                tmp_alpha =1.0f-(float)tmp_count / 5f;
            }
            foreach (var i in m_EPTexts)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, tmp_alpha);
            }
            foreach (var i in m_EPImages)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, tmp_alpha);
            }
            yield return new WaitForFixedUpdate();
        }
        CoroutineFade = null;
        if (!_fadeIn)
        {
            m_fadeDone = true;
        }
        else
        {
            m_fadeDone = false;
        }
    }
}

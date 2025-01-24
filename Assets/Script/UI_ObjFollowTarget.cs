using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_ObjFollowTarget : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    protected Vector3 m_posOnScreen;
    public bool m_fixPosition ;
    [SerializeField]protected Camera m_cam;
    void Start()
    {
        m_cam = Camera.main;
    }
    void Update()
    {
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
}

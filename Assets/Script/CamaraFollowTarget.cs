using UnityEngine;

public class CamaraFollowTarget : MonoBehaviour
{
    public Transform m_target;
    [SerializeField]Vector3 m_offset;
    public float m_speed;
    public float m_ZDistance = -15.0f;
    //------------------------------------------------------------------------------------------------------------
    void LateUpdate()
    {
        if (!m_target) return;
        if (transform.position != m_target.position)
        {
            var targetPos = Vector2.Lerp(transform.position, m_target.position + m_offset, m_speed * Time.deltaTime);
            transform.position = new Vector3(targetPos.x, targetPos.y, m_ZDistance);
        }
    }
}
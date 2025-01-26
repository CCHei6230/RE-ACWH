using UnityEngine;

public class SPSkillObj : MonoBehaviour
{
    [SerializeField] GameObject m_AtkPrefab;
    void Start()
    {
        Time.timeScale = 0;
    }
    void Anim_Done()
    {
        if (m_AtkPrefab)
        {
            Instantiate(m_AtkPrefab);
        }
        Destroy(gameObject);
        Time.timeScale = 1;
    }
}

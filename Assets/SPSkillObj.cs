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
            Instantiate(m_AtkPrefab,Camera.main.transform.position - new Vector3(0,0,-15f),Quaternion.identity);
        }
        else
        {
            Time.timeScale = 1;
        }
        Destroy(gameObject);
    }
}

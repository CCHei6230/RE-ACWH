using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_warningPrefab;
    [SerializeField] GameObject m_warning;
    [SerializeField] Transform m_BossSpawnPos;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_warning)
            {
                m_warning = Instantiate(m_warningPrefab,GameObject.Find("Overlayer").transform);
                m_warning.GetComponent<WarningObj>().SpawnPoint = m_BossSpawnPos;
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class WarningObj: MonoBehaviour
{
    [SerializeField] GameObject m_bossPrefab;
    public Transform SpawnPoint;
    void Start()
    {
        Time.timeScale = 0;
    }
    void Anim_Done()
    {
        Instantiate(m_bossPrefab,SpawnPoint.position,Quaternion.identity)
            .GetComponent<Boss>().AtkPos = SpawnPoint;
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
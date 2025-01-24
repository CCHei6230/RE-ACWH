using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]GameObject m_enemyToSpawn;
    [SerializeField]GameObject m_spawnEffect;
    [SerializeField]bool m_spawned = false;
    public bool Spawned
    {
        get => m_spawned;
        set => m_spawned = value;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_spawned)
            {
                Destroy(Instantiate(m_spawnEffect, transform.position , Quaternion.identity) , 2f);
                m_spawned = true; 
                Invoke("Spawne", 0.1f);
            }
        }
    }

    void Spawne()
    {
        Instantiate(m_enemyToSpawn , transform.position , Quaternion.identity);
    }

    public static void ResetSpawner()
    {
        var tmp_Spawner =  FindObjectsByType<EnemySpawner>(FindObjectsSortMode .None);
        foreach (var _spawner in tmp_Spawner)
        {
            _spawner.Spawned = false;
        }
        Debug.Log("Be Called ");
    }
}

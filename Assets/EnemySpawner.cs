using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]GameObject m_enemyToSpawn;
    [SerializeField]bool m_spawned = false;
    public bool Spawned
    {
        get => m_spawned;
        set => m_spawned = value;
    }
    [SerializeField] Animator m_anim;
    public Animator Anim
    {
        get => m_anim;
        set => m_anim = value;
    }
    readonly int Anim_Spawn = Animator.StringToHash("Spawn");
    static readonly int Anim_idle = Animator.StringToHash("Spawner_idle");
    private void Start()
    {
        m_anim.enabled = false;
    }
    void Anim_SpawnEnemy() 
    {
        Instantiate(m_enemyToSpawn, transform.position, Quaternion.identity);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_spawned)
            {
                m_spawned = true; 
                m_anim.enabled = true;
                m_anim.Rebind();
                m_anim.Update(0f);
                m_anim.CrossFade(Anim_Spawn,0);
            }
        }
    }
    public static void ResetSpawner()
    {
        var tmp_Spawner =  FindObjectsByType<EnemySpawner>(FindObjectsSortMode .None);
        foreach (var _spawner in tmp_Spawner)
        {
            _spawner.Spawned = false;
            _spawner.Anim.enabled = false;
        }
        Debug.Log("Be Called ");
    }
}

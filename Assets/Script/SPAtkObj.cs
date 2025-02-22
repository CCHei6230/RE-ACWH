using UnityEngine;

public class SPAtkObj : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }

    void Anim_SetTimeScaleToOne()
    {
        Time.timeScale = 1;
    }

    void Anim_Done()
    {
        Destroy(gameObject);
    }
     void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
               
                var tmp_enemy = other.transform.root.gameObject.GetComponent<EnemyBase>();
                tmp_enemy.TakeDamage(500);
                if (tmp_enemy.HP <= 0)
                {
                    var tmp_Score = FindFirstObjectByType<ScoreSystem>();
                    tmp_Score.ComboIncrease();
                    tmp_Score.ExtraFinish(true);
                }
                break;
        }
    }
}

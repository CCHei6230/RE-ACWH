using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField]  TMP_Text m_score;
    [SerializeField]  TMP_Text m_clearTime;
    [SerializeField]  TMP_Text m_rank;
    [SerializeField] SceneObject m_Title;
    void Start()
    {
        var tmp_resultData = FindFirstObjectByType<ResultData>();
        m_score.text = "Score : " +   tmp_resultData.Score.ToString();
        
        string tmp_timeString = string.Format("{00:00}{1:00}", Mathf.FloorToInt(tmp_resultData.ClearTime / 60), Mathf.FloorToInt(tmp_resultData.ClearTime % 60));
        string tmp_timeText = "";
        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
            {
                tmp_timeText += " : ";
            }
            tmp_timeText += tmp_timeString[i].ToString();
        }
        m_clearTime.text ="Time : " +  tmp_timeText;
        if (tmp_resultData.ClearTime < 300 )
        {
            m_rank.text = "S";
            m_rank.color = Color.yellow;
        }
        else if( tmp_resultData.ClearTime < 350)
        {
            m_rank.text = "A";
            m_rank.color = new Color32(255, 120, 0, 255);
        }
        else if( tmp_resultData.ClearTime  < 400)
        {
            m_rank.text = "B";
            m_rank.color = new Color32(0, 120, 250, 255);
        }
        else
        {
            m_rank.text = "C";
            m_rank.color = new Color32(120, 80, 30, 255);
        }
    }

    // Update is called once per frame
    void Anim_ToTitle()
    {
        SceneManager.LoadScene(m_Title);
    }
}

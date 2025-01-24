using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameTimer : MonoBehaviour
{
    public float InGameTime = 0;
    [SerializeField]TMP_Text m_timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InGameTime += Time.deltaTime;
        
        string tmp_timeString = string.Format("{00:00}{1:00}", Mathf.FloorToInt(InGameTime / 60), Mathf.FloorToInt(InGameTime % 60));
        string tmp_timeText = "";
        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
            {
                tmp_timeText += " : ";
            }

            tmp_timeText += tmp_timeString[i].ToString();
        }
            m_timerText.text = tmp_timeText;
    }
}

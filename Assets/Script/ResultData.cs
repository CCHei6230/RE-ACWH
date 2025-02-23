using UnityEngine;

public class ResultData: MonoBehaviour
{
    public float ClearTime;
    public int Score;

    public void SetData(float _inGameTime, int _score)
    {
        ClearTime = _inGameTime;
        Score = _score;
    }
}

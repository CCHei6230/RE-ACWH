using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextPointOrToResult : MonoBehaviour
{
    [SerializeField] SceneObject m_result = null;
    public bool ToResult = false;
    public Transform NextPosition;
    [SerializeField]Animator animator;
    readonly int Anim_FadeOut = Animator.StringToHash("UI_FadeOut");
    readonly int Anim_FadeIn = Animator.StringToHash("UI_FadeIn");

    public void FadeOut()
    {
        animator.CrossFade(Anim_FadeOut,0);
    }

    void Anim_ToNextPoint()
    {
        if (NextPosition)
        {
            FindAnyObjectByType<PlayerBehaviour>().transform.position = NextPosition.position;
            Camera.main.transform.position = NextPosition.position + new Vector3(0, 0.8f, -15f);
        }
    }
    public void Anim_ToResult( )
    {
        if (!ToResult)
        {
            return;
        }
        var tmp_scoreSys = FindFirstObjectByType<ScoreSystem>();
        var tmp_inGameTimer = FindFirstObjectByType<InGameTimer>();
        var tmp_resultData = new GameObject("Result Data").AddComponent<ResultData>();
        tmp_resultData.SetData(tmp_inGameTimer.InGameTime, tmp_scoreSys.Score);
        DontDestroyOnLoad(tmp_resultData.gameObject);
        SceneManager.LoadScene(m_result);
    }
}

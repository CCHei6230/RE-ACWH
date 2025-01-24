using UnityEngine;

public class TeleportToNextPoint : MonoBehaviour
{
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
        FindAnyObjectByType<PlayerBehaviour>().transform.position = NextPosition.position;
        Camera.main.transform.position = NextPosition.position + new Vector3(0, 0.8f, -15f);
    }
}

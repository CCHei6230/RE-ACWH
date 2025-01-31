using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]Animator animator;
    readonly int Anim_Out = Animator.StringToHash("CheckPoint_Out");
    public Transform NextPosition;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.CrossFade(Anim_Out,0);
        }
    }
    void Anim_NextSecen()
    {
        var tmp_ToNextPoint = FindAnyObjectByType<TeleportToNextPoint>();
        tmp_ToNextPoint.NextPosition = NextPosition;
        tmp_ToNextPoint.FadeOut();
    }
}

using UnityEngine;

public class PlayerAnimationState : MonoBehaviour
{
    public Animator Animator;
    public PlayerMovement PlayerMovement;
    readonly int Anim_Idle = Animator.StringToHash("Anim_PlayerIdle");
    readonly int Anim_Jump = Animator.StringToHash("Anim_PlayerJump");
    readonly int Anim_Falling = Animator.StringToHash("Anim_PlayerFalling");
    readonly int Anim_Grounding = Animator.StringToHash("Anim_PlayerGrounding");
    readonly int Anim_Running = Animator.StringToHash("Anim_PlayerRunning");
    readonly int Anim_Dash = Animator.StringToHash("Anim_PlayerDash");
    readonly int Anim_AirFlip = Animator.StringToHash("Anim_PlayerAirFlip");
    readonly int Anim_Damage = Animator.StringToHash("Anim_PlayerDamage");
    readonly int Anim_ShootDone = Animator.StringToHash("Anim_PlayerShootDone");
    public int CurrentState ;
    public int NextState ;
    
    
    //AnimationClip[] AnimClips;

    void Start()
    {
        CurrentState = Anim_Falling;
        //Animator.CrossFade(AnimClips[0].name,0,0);
    }
    public  int  NextAnimationState(bool _Attacking)
    {
        var tmp_atking = _Attacking?1:0;
        Animator.SetFloat("Attacking",tmp_atking);
        if (PlayerMovement.Damageing) {return Anim_Damage;}
        else
        {
            if (PlayerMovement.AirFliping)
            {
                return Anim_AirFlip;
            }
            else
            {
                if (PlayerMovement.IsGrounded)
                {
                    if (CurrentState == Anim_Falling)
                    {
                        return Anim_Grounding;
                    }
                    if (PlayerMovement.Dashing)
                    {
                        return Anim_Dash;
                    }
                    else
                    {
                        if (PlayerMovement.XVelocity == 0)
                        {
                            return Anim_Idle;
                        }
                        else
                        {
                            return Anim_Running;
                        }
                    }
                }
                else
                {
                    if (PlayerMovement.Dashing && !PlayerMovement.WasGrounded)
                    {
                        return Anim_Dash;
                    }
                    else
                    {
                        if (PlayerMovement.JumpCount > 0)
                        {
                            return Anim_Jump;
                        }
                        else
                        {
                            return Anim_Falling;
                        }
                    }
                }
            }
        }
        
    }
}

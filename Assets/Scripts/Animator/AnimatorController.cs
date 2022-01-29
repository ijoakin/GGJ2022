using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationId
{
    IDLE = 0,
    JUMP = 1,
    RUN = 2,
    ATTACK = 3,
    DIE = 4,
    HURT = 5 
}

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    public void Play(AnimationId animationId)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.Play(animationId.ToString());
    }

    public void Play(AnimationClip animationClip)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.Play(animationClip.name.ToString());
    }
}

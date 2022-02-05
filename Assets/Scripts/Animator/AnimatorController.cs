using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    public void Play(AnimationClip animationClip)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        //Debug.Log("Playing " + animationClip.name);

        // -1 and 0.0f prevents button smashing from animation ending triggers not being dispatched
        animator.Play(animationClip.name.ToString(), -1, 0.0f);
    }
}

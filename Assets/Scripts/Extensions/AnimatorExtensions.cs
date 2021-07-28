using UnityEngine;

public static class AnimatorExtensions
{
    public static void PlayImmediate(this Animator animator, string stateName)
    {
        animator.Play(stateName);
        animator.Update(0f);
    }

    public static void SetTriggerImmediate(this Animator animator, string triggerName)
    {
        animator.SetTrigger(triggerName);
        animator.Update(0f);
    }

    public static void SetTriggerImmediate(this Animator animator, int triggerID)
    {
        animator.SetTrigger(triggerID);
        animator.Update(0f);
    }

    public static bool IsPlaying(this Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }
}
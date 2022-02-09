using UnityEngine;

public class MonkZenTransformationState : PlayerState
{
    public float TransformationJump = 5.0f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        player.PushVertically(TransformationJump);
    }
    public void OnAnimationEndedZenTransformation()
    {
        player.ExecuteState<MonkZenState>();
    }
}

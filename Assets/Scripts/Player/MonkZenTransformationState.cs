using UnityEngine;

public class MonkZenTransformationState : PlayerState
{
    public float TransformationJump = 5.0f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        PlayerSounds.Instance.PlayTransformationZen();
        player.PushVertically(TransformationJump);
    }
    public void OnAnimationEndedZenTransformation()
    {
        player.ExecuteState<MonkZenState>();
    }
}

using UnityEngine;

public class PunkPunchState : PlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        //player.Punch();
        //PlayerSounds.Instance.PlayPunch();
    }

    public void OnAnimationEndedPunch()
    {
        player.ExecuteState<PunkIdleState>();
    }
}

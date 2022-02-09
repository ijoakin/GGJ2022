using UnityEngine;

public class MonkKickState : PlayerState
{
    public float KickHeight = 3.0f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        player.Punch();
        PlayerSounds.Instance.PlayKick();
        player.PushVertically(KickHeight);
    }

    public void OnAnimationEndedKick()
    {
        player.ExecuteState<MonkIdleState>();
    }
}

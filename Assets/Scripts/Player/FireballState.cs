public class FireballState : PlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public void OnAnimationEndedFireball()
    {
        player.ExecuteState<MonkIdleState>();
    }
}

public class FireballState : PlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        PlayerSounds.Instance.PlayTransformation();
    }

    public void OnAnimationEndedFireball()
    {
        player.ExecuteState<MonkIdleState>();
    }
}

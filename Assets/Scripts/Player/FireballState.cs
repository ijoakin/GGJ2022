public class FireballState : PlayerState
{
    public void OnAnimationEndedFireball()
    {
        player.ExecuteState<MonkIdleState>();
    }
}

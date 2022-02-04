public class MonkKickState : PlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        //player.Punch();
        PlayerSounds.Instance.PlayPunch();
    }

    public override void OnExitState()
    {
        //rigidbody.velocity = new Vector2(playerGameObject.MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);
    }

    public void OnAnimationEndedKick()
    {
        player.ExecuteState<MonkIdleState>();
    }
}

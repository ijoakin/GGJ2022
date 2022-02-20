using UnityEngine;

public class MonkRunState : PlayerState
{
    public float MoveFactor = 1.25f;

    public override void OnUpdateState()
    {
        player.MoveHorizontally(MoveFactor);

        if (playerRigidbody.velocity.x == 0)
        {
            player.ExecuteState<MonkIdleState>();
        }
        else if (player.Attack)
        {
            player.ExecuteState<MonkKickState>();
        }
    }
}

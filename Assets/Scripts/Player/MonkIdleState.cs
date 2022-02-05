using UnityEngine;

public class MonkIdleState : PlayerState
{
    public override void OnUpdateState()
    {
        player.MoveHorizontally();

        if (playerRigidbody.velocity.x != 0)
        {
            player.ExecuteState<MonkRunState>();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            player.ExecuteState<MonkKickState>();
        }
    }
}

using UnityEngine;

public class PunkIdleState : PlayerState
{
    public override void OnUpdateState()
    {
        player.MoveHorizontally();

        if (playerRigidbody.velocity.x != 0)
        {
            player.ExecuteState<PunkWalkState>();
        }
        else if (player.Attack)
        {
            player.ExecuteState<PunkPunchState>();
        }
    }
}

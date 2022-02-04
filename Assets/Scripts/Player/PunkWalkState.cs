using UnityEngine;

public class PunkWalkState : PlayerState
{
    public override void OnUpdateState()
    {
        player.MoveHorizontally();

        if (playerRigidbody.velocity.x == 0)
        {
            player.ExecuteState<PunkIdleState>();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            player.ExecuteState<PunkPunchState>();
        }
    }
}

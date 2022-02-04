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
        else if (Input.GetButtonDown("Fire1"))
        {
            player.ExecuteState<PunkPunchState>();
        }
    }
}

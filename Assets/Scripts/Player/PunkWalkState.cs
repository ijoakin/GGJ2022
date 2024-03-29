﻿using UnityEngine;

public class PunkWalkState : PlayerState
{
    public override void OnUpdateState()
    {
        player.MoveHorizontally();

        if (playerRigidbody.velocity.x == 0)
        {
            player.ExecuteState<PunkIdleState>();
        }
        else if (player.Attack)
        {
            player.ExecuteState<PunkPunchState>();
        }
    }
}

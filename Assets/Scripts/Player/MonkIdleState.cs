﻿using UnityEngine;

public class MonkIdleState : PlayerState
{
    public override void OnUpdateState()
    {
        player.MoveHorizontally();

        if (playerRigidbody.velocity.x != 0)
        {
            player.ExecuteState<MonkRunState>();
        }
        else if (player.Attack)
        {
            player.ExecuteState<MonkKickState>();
        }
    }
}

using UnityEngine;

public class MonkRunState : PlayerState
{
    public override void OnUpdateState()
    {
        playerRigidbody.velocity = new Vector2(player.MoveSpeed * Input.GetAxis("Horizontal"), playerRigidbody.velocity.y);

        if (Input.GetButtonDown("Fire1"))
        {
            player.ExecuteState<MonkKickState>();
        } else if (playerRigidbody.velocity.x == 0)
        {
            player.ExecuteState<MonkIdleState>();
        }
    }
}

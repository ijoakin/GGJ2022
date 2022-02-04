using UnityEngine;

public class MonkZenState : PlayerState
{
    public override void OnUpdateState()
    {
        playerRigidbody.velocity = new Vector2(player.MoveSpeed * Input.GetAxis("Horizontal"), playerRigidbody.velocity.y);

        if (playerRigidbody.velocity.x > 0)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (this.playerRigidbody.velocity.x < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
    }
}

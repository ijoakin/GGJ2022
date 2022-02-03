using System.Collections;
using UnityEngine;

public class MonkZenContinueState : PlayerState
{
    private float waitDuration = 0.5f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }
    public override void OnUpdateState()
    {
        rigidbody.velocity = new Vector2(playerGameObject.MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        if (rigidbody.velocity.x > 0)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (this.rigidbody.velocity.x < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
    }
}

using System.Collections;
using UnityEngine;

public class MonkRunState : PlayerState
{
    private float waitDuration = 2;

    public override void OnEnterState()
    {
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
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
        playerGameObject.isWalking = false;
    }
}

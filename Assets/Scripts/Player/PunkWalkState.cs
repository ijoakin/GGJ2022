using System.Collections;
using UnityEngine;

public class PunkWalkState : PlayerState
{
    private float waitDuration = 2f;

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
            playerSpriteRenderer.flipX = true;
        }
        else if (this.rigidbody.velocity.x < 0)
        {
            playerSpriteRenderer.flipX = false;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        playerGameObject.isWalking = false;
    }
}

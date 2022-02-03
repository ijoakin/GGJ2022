using System.Collections;
using UnityEngine;

public class PunkWalkState : PlayerState
{
    private float waitDuration = 2f;

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
            playerSpriteRenderer.flipX = true;
        }
        else if (this.rigidbody.velocity.x < 0)
        {
            playerSpriteRenderer.flipX = false;
        }
        else
        {
            playerGameObject.ExecuteState<PunkIdleState>();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerGameObject.ExecuteState<PunkPunchState>();
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
    }
}

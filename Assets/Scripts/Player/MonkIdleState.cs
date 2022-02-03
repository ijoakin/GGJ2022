using System.Collections;
using UnityEngine;

public class MonkIdleState : PlayerState
{
    private float waitDuration = 0.1f;

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

        if (rigidbody.velocity.x != 0)
        {
            playerGameObject.ExecuteState<MonkRunState>();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerGameObject.ExecuteState<MonkKickState>();
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
    }
}

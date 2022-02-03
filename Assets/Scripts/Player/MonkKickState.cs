using System.Collections;
using UnityEngine;

public class MonkKickState : PlayerState
{
    private float waitDuration = .5f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        playerGameObject.Punch();
        PlayerSounds.Instance.PlayPunch();
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
        rigidbody.velocity = new Vector2(playerGameObject.MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        playerGameObject.ExecuteState<MonkIdleState>();
    }
}

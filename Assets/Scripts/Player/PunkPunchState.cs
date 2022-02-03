using System.Collections;
using UnityEngine;

public class PunkPunchState : PlayerState
{
    private float waitDuration = 0.5f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        playerGameObject.Punch();
        PlayerSounds.Instance.PlayPunch();
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }
    public override void OnUpdateState()
    {
        rigidbody.velocity = new Vector2(playerGameObject.MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        this.stateIsFinished = true;
        playerGameObject.ExecuteState<PunkIdleState>();
    }
}

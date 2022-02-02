using System.Collections;
using UnityEngine;

public class MonkKickState : PlayerState
{
    private float waitDuration = .5f;

    public override void OnEnterState()
    {
        playerGameObject.Punch();
        PlayerSounds.Instance.PlayPunch();
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        playerGameObject.isPunching = false;
    }
}

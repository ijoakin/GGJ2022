using System.Collections;
using UnityEngine;

public class MonkIdleState : PlayerState
{
    private float waitDuration = 0.1f;

    public override void OnEnterState()
    {
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        playerGameObject.isIdle = false;
    }
}

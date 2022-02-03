using System.Collections;
using System.Threading;
using UnityEngine;

public class FireballState : PlayerState
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

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        stateIsFinished = true;
        playerGameObject.ExecuteState<MonkIdleState>();
    }
}

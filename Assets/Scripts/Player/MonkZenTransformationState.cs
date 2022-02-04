using System.Collections;
using UnityEngine;

public class MonkZenTransformationState : PlayerState
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

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
        player.ExecuteState<MonkZenState>();
    }
}

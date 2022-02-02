using System.Collections;
using UnityEngine;

public class MonkZenState : PlayerState
{
    private float waitDuration = 0.5f;

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
        playerGameObject.isConvertingToAang = false;
        playerGameObject.isAvatarMode = true;
        playerGameObject.ExecuteState<MonkZenContinueState>();
    }
}

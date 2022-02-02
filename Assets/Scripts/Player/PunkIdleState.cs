using System.Collections;
using UnityEngine;

public class PunkIdleState : PlayerState
{
    private float waitDuration = 0.2f;

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

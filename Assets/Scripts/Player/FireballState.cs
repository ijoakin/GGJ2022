using System.Collections;
using System.Threading;
using UnityEngine;

public class FireballState : PlayerState
{
    private float waitDuration = 2f;

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
        playerGameObject.isFireball = false;

        playerGameObject.ExecuteState<MonkIdleState>();
    }
}

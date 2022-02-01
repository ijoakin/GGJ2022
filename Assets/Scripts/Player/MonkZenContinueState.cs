using System.Collections;
using UnityEngine;

public class MonkZenContinueState : PlayerState
{
    [SerializeField] private float waitDuration = 2;

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
        playerGameObject.StateFinished();
    }
}

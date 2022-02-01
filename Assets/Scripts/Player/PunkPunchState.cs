using System.Collections;
using UnityEngine;

public class PunkPunchState : PlayerState
{
    [SerializeField] private float waitDuration = 2;

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
        playerGameObject.StateFinished();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : EnemyState
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
        enemyGameObject.StateFinished();
    }
}

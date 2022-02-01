﻿using System.Collections;
using System.Threading;
using UnityEngine;

public class FireballState : PlayerState
{
    [SerializeField] private float waitDuration = 20f;

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

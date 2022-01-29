using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonEnemyController : EnemyGameObject
{
    public int Life { get; set; }

    public override void OnAwake()
    {
        ExecuteState<ChasePlayerState>();
    }

    public override void OnUpdate()
    {
        if (stateFinished && CurrentStateIs<ChasePlayerState>())
        {
            ExecuteState<IdleEnemyState>();
        }
        if (stateFinished && CurrentStateIs<IdleEnemyState>())
        {
            ExecuteState<ChasePlayerState>();
        }
    }
}

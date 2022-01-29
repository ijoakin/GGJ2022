using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField] private AnimationClip animation;

    protected Rigidbody2D _rigidbody2D;
    protected EnemyGameObject enemyGameObject;

    public void SetRigidBody2D(Rigidbody2D _rigidbody2D)
    {
        this._rigidbody2D = _rigidbody2D;
    }

    public void SetEnemyGameObject(EnemyGameObject enemyGameObject)
    {
        this.enemyGameObject = enemyGameObject;
    }

    public AnimationClip GetAnimationClip()
    {
        return animation;
    }

    public abstract void OnEnterState();

    public virtual void OnUpdateState()
    {
    }

    public abstract void OnExitState();

}

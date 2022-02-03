﻿using UnityEngine;

public abstract class PlayerState: MonoBehaviour
{
    [SerializeField] private AnimationClip animation;

    protected Rigidbody2D rigidbody;
    protected Player playerGameObject;
    protected SpriteRenderer playerSpriteRenderer;
    protected bool stateIsFinished;

    public void SetSpriteRenderer(SpriteRenderer _spriteRenderer)
    {
        playerSpriteRenderer = _spriteRenderer;
    }

    public void SetRigidBody2D(Rigidbody2D _rigidbody2D)
    {
        this.rigidbody = _rigidbody2D;
    }

    public void SetEnemyGameObject(Player playerGameObject)
    {
        this.playerGameObject = playerGameObject;
    }

    public AnimationClip GetAnimationClip()
    {
        return animation;
    }

    public virtual void OnEnterState()
    {
        stateIsFinished = false;
    }

    public virtual void OnUpdateState()
    {
    }

    public abstract void OnExitState();
}


using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public AnimationClip playerAnimation;

    protected Player player;
    protected Rigidbody2D playerRigidbody;
    protected SpriteRenderer playerSpriteRenderer;

    public void SetSpriteRenderer(SpriteRenderer _spriteRenderer)
    {
        playerSpriteRenderer = _spriteRenderer;
    }

    public void SetRigidBody2D(Rigidbody2D _rigidbody2D)
    {
        this.playerRigidbody = _rigidbody2D;
    }

    public void SetEnemyGameObject(Player playerGameObject)
    {
        this.player = playerGameObject;
    }

    public AnimationClip GetAnimationClip()
    {
        return playerAnimation;
    }

    public virtual void OnEnterState()
    {
    }

    public virtual void OnExitState()
    {
    }

    public virtual void OnUpdateState()
    {
    }
}


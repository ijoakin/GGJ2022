using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : EnemyState
{
    [SerializeField] private float speed;
    [SerializeField] private bool canFly = false;
    [SerializeField] private Vector2 chaseOffset;
    public float distanceToTarget = 0;
    public Vector2 _chaseOffset;
    private Vector2 direction;

    public override void OnEnterState()
    {
        _chaseOffset = chaseOffset;
    }

    private void CalculateChaseOffset()
    {
        _chaseOffset = new Vector2(chaseOffset.x * -1 * Mathf.Sign(direction.x), chaseOffset.y * -1 * Mathf.Sign(direction.y));
        if (canFly)
        {
            _chaseOffset.y = chaseOffset.y;
        }
    }

    public override void OnUpdateState()
    {
        CalculateChaseOffset();
        //direction = (PlayerController.instance.transform.position - this.transform.position + (Vector3)_chaseOffset).normalized;
        //distanceToTarget = Vector2.Distance(PlayerController.instance.transform.position + (Vector3)_chaseOffset, this.transform.position);

        if (distanceToTarget > 0.5f)
        {
            if (canFly)
            {
                _rigidbody2D.velocity = direction * speed;
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(direction.x * speed, _rigidbody2D.velocity.y);
            }
        }
        else
        {
            enemyGameObject.StateFinished();

            if (canFly)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
        }
    }

    public override void OnExitState()
    {
    }
}

using System.Collections;
using UnityEngine;

public class PunkIdleState : PlayerState
{
    private float waitDuration = 0.2f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        StartCoroutine(Wait());
    }

    public override void OnExitState()
    {
    }
    public override void OnUpdateState()
    {

        rigidbody.velocity = new Vector2(playerGameObject.MoveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

        if(rigidbody.velocity.x != 0)
        {
            playerGameObject.ExecuteState<PunkWalkState>();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerGameObject.ExecuteState<PunkPunchState>();
        }

        if ((playerGameObject.chargeCount >= 0))
        {
            playerGameObject.chargeCount -= Time.deltaTime;
            if (playerGameObject.chargeCount <= 0)
            {
                playerGameObject.Charge(10);
                playerGameObject.chargeCount = playerGameObject.chargeLenght;
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitDuration);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunkHurtState : PlayerState
{

    public override void OnEnterState()
    {
        base.OnEnterState();
       player.spriteRenderer.flipX = true;
        //player.Punch();
        PlayerSounds.Instance.PlayPunch();
    }

    public void OnAnimationEndedPunch()
    {
        //player.ExecuteState<PunkIdleState>();
        Debug.Log("Punk recibio damage");
        //TODO: Move to OnCollide()
        //AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId., AudioManager.AudioId.PUNK_PINA_5);
    }
}

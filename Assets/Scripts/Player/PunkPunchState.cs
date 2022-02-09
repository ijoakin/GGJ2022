using UnityEngine;

public class PunkPunchState : PlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        player.Punch();
        PlayerSounds.Instance.PlayPunch();
    }

    public void OnAnimationEndedPunch()
    {
        player.ExecuteState<PunkIdleState>();

        //TODO: Move to OnCollide()
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_PINA_1, AudioManager.AudioId.PUNK_PINA_5);
    }
}

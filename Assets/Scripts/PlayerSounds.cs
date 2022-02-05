using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public static PlayerSounds Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayJumpMonk()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.MONJE_SALTA_1, AudioManager.AudioId.MONJE_SALTA_2);
    }

    public void PlayJumpPunk()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_SALTO_1, AudioManager.AudioId.PUNK_SALTO_3);
    }

    public void PlayMiss()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_ERRA_1, AudioManager.AudioId.PUNK_ERRA_4);
    }

    public void PlayPunch()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_PINA_1, AudioManager.AudioId.PUNK_PINA_5);
    }

    public void PlayRage80()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_IRA_AL_80_1, AudioManager.AudioId.PUNK_IRA_AL_80_3);
    }

    public void PlayTransformation()
    {
        AudioManager.Instance.PlaySFX(AudioManager.AudioId.MONJE_TRANS);
    }

    public void PlayTransformationZen()
    {
        AudioManager.Instance.PlaySFX(AudioManager.AudioId.MONJE_TRANS);
    }
}

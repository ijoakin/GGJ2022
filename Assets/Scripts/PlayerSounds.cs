using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public static PlayerSounds Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayJumpPunk()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_SALTO_1, AudioManager.AudioId.PUNK_SALTO_3);
    }

    public void PlayKick()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.MONJE_PATADA_1, AudioManager.AudioId.MONJE_PATADA_2);
    }

    public void PlayMiss()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_ERRA_1, AudioManager.AudioId.PUNK_ERRA_4);
    }

    public void PlayPunch()
    {
        AudioManager.Instance.PlaySFXRandom(AudioManager.AudioId.PUNK_ERRA_1, AudioManager.AudioId.PUNK_ERRA_5);
    }
}

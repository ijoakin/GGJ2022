using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerSounds Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPunch()
    {
        AudioManager.Instance.PlayRandom(AudioManager.AudioId.PUNK_PINA_1, AudioManager.AudioId.PUNK_PINA_5);
    }
}

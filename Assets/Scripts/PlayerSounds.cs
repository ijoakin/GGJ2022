using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerSounds instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayPunch()
    {
        var audioId = Random.Range(10, 14);
        AudioManager.instance.PlaySFX(audioId);
    }
}

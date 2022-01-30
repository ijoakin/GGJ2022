using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] soundEffects;

    [SerializeField] private AudioSource sfxAudioSource;

    public AudioSource BGMusic;
    public AudioSource levelEndMusic;

    public enum AudioId
    {
        MONJESALTA1 = 0,
        MONJESALTA2 = 1,
        MONJETRANS = 2,
        PUNKERRA1 = 3,
        PUNKERRA2 = 4,
        PUNKERRA3 = 5,
        PUNKERRA4 = 6,
        PUNKIRAAL80 = 7,
        PUNKIRAAL80_2 = 8,
        PUNKIRAAL80_3 = 9,

        PUNKPINA1 = 10,
        PUNKPINA2 = 11,
        PUNKPINA3 = 12,
        PUNKPINA4 = 13,
        PUNKPINA5 = 14,

        PUNKSALTO1 = 15,
        PUNKSALTO2 = 16,
        PUNKSALTO3 = 17,
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();

        soundEffects[soundToPlay].pitch = Random.Range(.9f, 1.1f);

        soundEffects[soundToPlay].Play();
    }


    public void PlaySfxByAudioClip(AudioClip sfx)
    {
        //sfxAudioSource.PlayOneShot(sfx);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource[] MusicFiles;
    public AudioSource[] SoundEffects;

    private AudioSource currentMusic;
    private AudioId currentMusicId;

    //[SerializeField]
    //private AudioSource sfxAudioSource;
    //public AudioSource BGMusic;
    //public AudioSource levelEndMusic;

    public enum AudioId
    {
        MONJE_SALTA_1,
        MONJE_SALTA_2,

        MONJE_TRANS,

        PUNK_ERRA_1,
        PUNK_ERRA_2,
        PUNK_ERRA_3,
        PUNK_ERRA_4,

        PUNK_IRA_AL_80_1,
        PUNK_IRA_AL_80_2,
        PUNK_IRA_AL_80_3,

        PUNK_PINA_1,
        PUNK_PINA_2,
        PUNK_PINA_3,
        PUNK_PINA_4,
        PUNK_PINA_5,

        PUNK_SALTO_1,
        PUNK_SALTO_2,
        PUNK_SALTO_3,

        LAST
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMusicId = AudioId.LAST;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMusic && !currentMusic.isPlaying)
        {
            currentMusic.Play();
        }
    }

    public void PlayRandom(AudioId audioMin, AudioId audioMax)
    {
        int indexMin = (int)audioMin;
        int indexMax = (int)audioMax;
        if (indexMax <= indexMin)
        {
            Debug.Log($"PlayRandom ERROR: indexMax <= indexMin ({indexMax}) ({indexMin})");
            return;
        }
        PlaySFX(Random.Range(indexMin, indexMax));
    }

    public void PlayMusicRandom()
    {
    }

    public void PlayMusic(AudioId audioId)
    {
        if (currentMusicId != AudioId.LAST)
        {
            SoundEffects[(int)currentMusicId].Stop();
        }

        currentMusicId = audioId;
        currentMusic = SoundEffects[(int)currentMusicId];
        currentMusic.Play();
    }

    public void PlaySFX(AudioId audioId)
    {
        PlaySFX((int)audioId);
    }

    public void PlaySFX(int soundToPlay)
    {
        SoundEffects[soundToPlay].Stop();
        SoundEffects[soundToPlay].pitch = Random.Range(.9f, 1.1f);
        SoundEffects[soundToPlay].Play();
    }

    //public void PlaySfxByAudioClip(AudioClip sfx)
    //{
    //    sfxAudioSource.PlayOneShot(sfx);
    //}
}

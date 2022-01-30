using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool DebugNextMusicId;

    public AudioSource[] MusicFiles;
    public AudioSource[] SoundEffects;

    private AudioSource currentMusic;
    private AudioSource nextMusic;
    private MusicId nextMusicId;

    public enum AudioId
    {
        PUNK_SALTO_1,
        PUNK_SALTO_2,
        PUNK_SALTO_3,

        PUNK_IRA_AL_80_1,
        PUNK_IRA_AL_80_2,
        PUNK_IRA_AL_80_3,

        PUNK_ERRA_1,
        PUNK_ERRA_2,
        PUNK_ERRA_3,
        PUNK_ERRA_4,
        PUNK_ERRA_5,

        MONJE_SALTA_1,
        MONJE_SALTA_2,

        MONJE_TRANS,

        PUNK_PINA_1,
        PUNK_PINA_2,
        PUNK_PINA_3,
        PUNK_PINA_4,
        PUNK_PINA_5,

        ENEMIGO_MUERE_1,
        ENEMIGO_MUERE_2,
        ENEMIGO_MUERE_3,

        ENEMIGO_SANGRE_1,
        ENEMIGO_SANGRE_2,
        ENEMIGO_SANGRE_3,
        ENEMIGO_SANGRE_4,
        ENEMIGO_SANGRE_5,

        LAST
    }

    public enum MusicId
    {
        PUNK_DOWN_1,
        PUNK_DOWN_2,
        PUNK_DOWN_3,
        PUNK_DOWN_4,
        PUNK_DOWN_5,
        PUNK_DOWN_6,
        PUNK_DOWN_7,
        PUNK_UP_1,
        PUNK_UP_2,
        PUNK_UP_3,
        PUNK_UP_4,
        PUNK_UP_5,
        PUNK_UP_6,
        TRANS,
        MONK,
        MONK_ZEN,
        LAST
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextMusicId = MusicId.LAST;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugNextMusicId)
        {
            Debug.Log(nextMusicId);
        }

        if (currentMusic && !currentMusic.isPlaying)
        {
            playNext();
        }
        else if (!currentMusic)
        {
            playNext();
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

    public void PlayMusicRandom(MusicId audioMin, MusicId audioMax)
    {
        int indexMin = (int)audioMin;
        int indexMax = (int)audioMax;
        if (indexMax <= indexMin)
        {
            Debug.Log($"PlayMusicRandom ERROR: indexMax <= indexMin ({indexMax}) ({indexMin})");
            return;
        }
        PlayMusic(Random.Range(indexMin, indexMax));
    }

    public void PlayMusic(MusicId musicId)
    {
        nextMusicId = musicId;
        PlayMusic((int)musicId);
    }

    public void PlayMusic(int musicId)
    {
        nextMusic = MusicFiles[musicId];
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

    private void playNext()
    {
        currentMusic = nextMusic;
        if (currentMusic)
        {
            currentMusic.Play();
        }
    }
}

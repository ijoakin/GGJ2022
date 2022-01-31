using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource[] MusicFiles;
    public AudioSource[] SoundEffects;

    private MusicPlayerController musicPlayerController;

    private int randomMin;
    private int randomMax;

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
        musicPlayerController = new MusicPlayerController(this);
    }

    // Update is called once per frame
    void Update()
    {
        musicPlayerController.Update();
    }

    public void PlayMusic(MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        musicPlayerController.PlayMusic(_musicId, _transMusicId);
    }

    public void PlayMusicLoop(MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        musicPlayerController.PlayMusicLoop(_musicId, _transMusicId);
    }

    public void PlayMusicNext()
    {
        musicPlayerController.PlayMusicNext();
    }

    public void PlayMusicRandomLoop(AudioManager.MusicId _randomMin, AudioManager.MusicId _randomMax, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        musicPlayerController.PlayMusicRandomLoop((int)_randomMin, (int)_randomMax, _transMusicId);
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

    public void PlaySFXRandom(AudioId audioMin, AudioId audioMax)
    {
        randomMin = (int)audioMin;
        randomMax = (int)audioMax;
        if (randomMax <= randomMin)
        {
            Debug.Log($"PlaySFXRandom ERROR: audioMax <= audioMin ({randomMax}) ({randomMin})");
            return;
        }
        PlaySFX(Random.Range(randomMin, randomMax + 1));
    }
}

public class MusicPlayerController
{
    private AudioManager audioManager;
    private MusicPlayer currentPlayer;
    private AudioManager.MusicId musicId;
    private List<MusicPlayer> musicPlayers;
    private PlayerTypes playerType;
    private int randomMin;
    private int randomMax;
    private MusicPlayer simpleMusicPlayer;
    private AudioManager.MusicId transMusicId;


    private enum PlayerTypes
    {
        SIMPLE,
        LOOP,
        RANDOM_LOOP
    }

    public MusicPlayerController(AudioManager _audioManager)
    {
        audioManager = _audioManager;

        musicPlayers = new List<MusicPlayer>();
        musicPlayers.Add(new SimpleMusicPlayer(audioManager));
        musicPlayers.Add(new LoopMusicPlayer(audioManager));
        musicPlayers.Add(new RandomLoopMusicPlayer(audioManager));
        currentPlayer = musicPlayers[0];
        simpleMusicPlayer = musicPlayers[0];
    }

    public void PlayMusic(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        playerType = PlayerTypes.SIMPLE;
        startPlaying(_musicId, _transMusicId);
    }

    public void PlayMusicLoop(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        playerType = PlayerTypes.LOOP;
        startPlaying(_musicId, _transMusicId);
    }

    public void PlayMusicNext()
    {
        currentPlayer.Stop();
        currentPlayer.Next();
    }

    public void PlayMusicRandomLoop(int _randomMin, int _randomMax, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        playerType = PlayerTypes.RANDOM_LOOP;

        musicId = AudioManager.MusicId.LAST;
        randomMin = _randomMin;
        randomMax = _randomMax;
        transMusicId = _transMusicId;

        currentPlayer.Stop();
        currentPlayer = musicPlayers[(int)playerType];

        if (transMusicId != AudioManager.MusicId.LAST)
        {
            simpleMusicPlayer.Play(transMusicId);
        }
        else
        {
            currentPlayer.Play(randomMin, randomMax);
        }
    }

    private void startPlaying(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        musicId = _musicId;
        transMusicId = _transMusicId;

        currentPlayer.Stop();
        currentPlayer = musicPlayers[(int)playerType];

        if (transMusicId != AudioManager.MusicId.LAST)
        {
            simpleMusicPlayer.Play(transMusicId);
        }
        else
        {
            currentPlayer.Play(musicId);
        }
    }

    public void Update()
    {
        if (transMusicId != AudioManager.MusicId.LAST
            && simpleMusicPlayer.CurrentMusic
            && !simpleMusicPlayer.CurrentMusic.isPlaying)
        {
            transMusicId = AudioManager.MusicId.LAST;
            if (playerType == PlayerTypes.RANDOM_LOOP)
            {
                currentPlayer.Play(randomMin, randomMax);
            }
            else
            {
                currentPlayer.Play(musicId);
            }
        }
        else if (transMusicId == AudioManager.MusicId.LAST)
        {
            currentPlayer.Update();
        }
    }
}

public abstract class MusicPlayer
{
    protected AudioManager audioManager;
    public AudioSource CurrentMusic;
    protected AudioManager.MusicId musicId;
    protected AudioSource transMusic;
    protected AudioManager.MusicId transMusicId;

    public MusicPlayer(AudioManager _audioManager)
    {
        audioManager = _audioManager;
    }

    public virtual void Next()
    {
    }

    public virtual void Play(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        musicId = _musicId;
        transMusicId = _transMusicId;
    }

    public virtual void Play(int _randomMin, int _randomMax, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        transMusicId = _transMusicId;
    }

    public void Stop()
    {
        if (!CurrentMusic)
        {
            return;
        }
        CurrentMusic.Stop();
    }

    public virtual void Update()
    {
    }
}

public class SimpleMusicPlayer : MusicPlayer
{
    public SimpleMusicPlayer(AudioManager _audioManager)
        : base(_audioManager)
    {
    }

    override public void Play(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        base.Play(_musicId);
        //Debug.Log($"SimpleMusicPlayer Play {_musicId}");
        CurrentMusic = audioManager.MusicFiles[(int)_musicId];
        CurrentMusic.Play();
    }
}

public class LoopMusicPlayer : MusicPlayer
{
    public LoopMusicPlayer(AudioManager _audioManager)
        : base(_audioManager)
    {
    }

    override public void Next()
    {
        CurrentMusic.Play();
    }

    override public void Play(AudioManager.MusicId _musicId, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        base.Play(_musicId);
        //Debug.Log($"LoopMusicPlayer Play {_musicId}");
        CurrentMusic = audioManager.MusicFiles[(int)_musicId];
        CurrentMusic.Play();
    }

    override public void Update()
    {
        if (CurrentMusic && !CurrentMusic.isPlaying)
        {
            //Debug.Log($"LoopMusicPlayer PlayLoop");
            CurrentMusic.Play();
        }
    }
}

public class RandomLoopMusicPlayer : MusicPlayer
{
    private int currentId;
    private int randomMin;
    private int randomMax;

    public RandomLoopMusicPlayer(AudioManager _audioManager)
        : base(_audioManager)
    {
    }

    override public void Next()
    {
        selectAndPlay();
    }

    override public void Play(int _randomMin, int _randomMax, AudioManager.MusicId _transMusicId = AudioManager.MusicId.LAST)
    {
        base.Play(_randomMin, _randomMax, _transMusicId);
        randomMin = _randomMin;
        randomMax = _randomMax;
        selectAndPlay();
    }

    private void selectAndPlay()
    {
        if (!selectNextId())
        {
            return;
        }
        //Debug.Log($"RandomLoopMusicPlayer Play {currentId}");
        CurrentMusic = audioManager.MusicFiles[currentId];
        CurrentMusic.Play();
    }

    private bool selectNextId()
    {
        if (randomMax <= randomMin)
        {
            //Debug.Log($"RandomLoopMusicPlayer Play ERROR: randomMax <= randomMin ({randomMax}) ({randomMin})");
            return false;
        }
        currentId = Random.Range(randomMin, randomMax + 1);
        return true;
    }

    override public void Update()
    {
        if (CurrentMusic && !CurrentMusic.isPlaying)
        {
            selectAndPlay();
        }
    }
}
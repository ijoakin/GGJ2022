using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    public bool DebugCurrentCharge;

    [Range(-100, 100)]
    public int CurrentCharge = 0;

    public enum ChargeStates
    {
        FURY,
        PUNK,
        MONK,
        ZEN
    }

    public ChargeStates ChargeState;

    private const int THRESHOLD_FURY = -40;
    private const int THRESHOLD_MONK = 50;
    private const int THRESHOLD_ZEN = 90;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChargeState = ChargeStates.PUNK;
        AudioManager.Instance.PlayMusicRandomLoop(AudioManager.MusicId.PUNK_DOWN_1, AudioManager.MusicId.PUNK_DOWN_7);
    }
    #region "Debug"
    //TODO: Remove all of this methods 
    public void HandleC()
    {
        Charge2(10);
    }
    public void HandleZ()
    {
        Charge2(-10);
    }

    public void HandleEscape()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void HandleX()
    {
        AudioManager.Instance.PlayMusicNext();
    }


    #endregion

    // Update is called once per frame
    void Update()
    {
        /*
        if (player.chargeCount >= 0)
        {
            player.chargeCount -= Time.deltaTime;
            if (player.chargeCount <= 0)
            {
                player.Charge(10);
                player.chargeCount = player.chargeLenght;
            }
        }
        */
    }

    public void Charge2(int value)
    {
        CurrentCharge = Bar.Instance.Charge(value);
        CheckCharge();
    }

    public void Charge(int value)
    {
    }

    public void CheckCharge()
    {
        if (DebugCurrentCharge)
        {
            Debug.Log(CurrentCharge);
        }

        if (CurrentCharge >= THRESHOLD_ZEN)
        {
            if (ChargeState != ChargeStates.ZEN)
            {
                ChargeState = ChargeStates.ZEN;
                Player.Instance.ConvertToZen();
                AudioManager.Instance.PlaySFX(AudioManager.AudioId.MONJE_AL_100, false);
                AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.MONJE_ZEN);
            }
        }
        else if (CurrentCharge <= THRESHOLD_FURY)
        {
            if (ChargeState != ChargeStates.FURY)
            {
                ChargeState = ChargeStates.FURY;

                int transitionMusic = Random.Range((int)AudioManager.MusicId.PUNK_IRA_1, (int)AudioManager.MusicId.PUNK_IRA_3 + 1);
                AudioManager.Instance.PlayMusicRandomLoop(AudioManager.MusicId.PUNK_UP_1, AudioManager.MusicId.PUNK_UP_6, (AudioManager.MusicId)transitionMusic);
            }
        }
        // Punk
        else if (CurrentCharge >= THRESHOLD_MONK && CurrentCharge < THRESHOLD_ZEN)
        {
            if (ChargeState != ChargeStates.MONK)
            {
                if (ChargeState == ChargeStates.PUNK)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.AudioId.MONJE_TRANSFORMACION, false);
                    AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.MONJE);
                }
                else if (ChargeState == ChargeStates.ZEN)
                {
                    AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.MONJE, AudioManager.MusicId.ZEN_A_MONJE);
                }

                ChargeState = ChargeStates.MONK;
                Player.Instance.ConvertToMonk();
            }
        }
        else if (CurrentCharge < THRESHOLD_MONK)
        {
            if (ChargeState != ChargeStates.PUNK)
            {
                AudioManager.Instance.PlayMusicRandomLoop(AudioManager.MusicId.PUNK_DOWN_1, AudioManager.MusicId.PUNK_DOWN_7, AudioManager.MusicId.MONJE_A_PUNK);
                ChargeState = ChargeStates.PUNK;
                Player.Instance.ConvertToPunk();
            }
        }
    }
}

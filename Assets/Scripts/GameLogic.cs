using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Charge2(10);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Charge2(-10);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            AudioManager.Instance.PlayMusicNext();
        }


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
                AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.MONK_ZEN, AudioManager.MusicId.TRANS);
            }
        }
        else if (CurrentCharge <= THRESHOLD_FURY)
        {
            if (ChargeState != ChargeStates.FURY)
            {
                ChargeState = ChargeStates.FURY;
                AudioManager.Instance.PlayMusicRandomLoop(AudioManager.MusicId.PUNK_UP_1, AudioManager.MusicId.PUNK_UP_6, AudioManager.MusicId.TRANS);
            }
        }
        else if (CurrentCharge >= THRESHOLD_MONK && CurrentCharge < THRESHOLD_ZEN)
        {
            if (ChargeState != ChargeStates.MONK)
            {
                ChargeState = ChargeStates.MONK;
                Player.Instance.ConvertToMonk();
                AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.MONK, AudioManager.MusicId.TRANS);
            }
        }
        else if (CurrentCharge < THRESHOLD_MONK)
        {
            if (ChargeState != ChargeStates.PUNK)
            {
                ChargeState = ChargeStates.PUNK;
                Player.Instance.ConvertToPunk();
                AudioManager.Instance.PlayMusicRandomLoop(AudioManager.MusicId.PUNK_DOWN_1, AudioManager.MusicId.PUNK_DOWN_7, AudioManager.MusicId.TRANS);
            }
        }
    }
}

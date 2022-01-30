using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    public bool DebugCurrentCharge;

    [SerializeField]
    [Range(-100, 100)]
    public int currentCharge = 0;

    private int valueMonk = 41;
    private int valueZen = 91;

    private int FuryPunk = -40;
    private int Monk = 41;
    private int MonkZen = 91;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayMusicByCharge();
    }

    public void Charge(int value)
    {
        //TODO: call the bar to charge and get the currentValue
        //currentCharge = 50;

        //currentCharge = Bar.Instance.CurrentCharge;
        Bar.Instance.Charge(value);
        CheckCharge();
    }

    public void CheckCharge()
    {
        currentCharge = Bar.Instance.CurrentCharge;
        if (DebugCurrentCharge)
        {
            Debug.Log(currentCharge);
        }
        
        if (currentCharge >= valueZen)
        {
            Player.Instance.ConvertToZen();
        }
        else if (currentCharge >= valueMonk && currentCharge < valueZen)
        {
            Player.Instance.ConvertToMonk();
        }
        else if (currentCharge < valueMonk)
        {
            Player.Instance.ConvertToPunk();
        }
    }

    public void PlayMusicByCharge()
    {
        currentCharge = Bar.Instance.CurrentCharge;
        if (currentCharge < FuryPunk)
        {
            AudioManager.Instance.PlayMusicRandom(AudioManager.MusicId.PUNK_UP_1, AudioManager.MusicId.PUNK_UP_6);
        }
        else if (currentCharge > MonkZen)
        {
            AudioManager.Instance.PlayMusic(AudioManager.MusicId.MONK_ZEN);
        }
        else if (currentCharge > Monk)
        {
            AudioManager.Instance.PlayMusic(AudioManager.MusicId.MONK);
        }
        else
        {
            AudioManager.Instance.PlayMusicRandom(AudioManager.MusicId.PUNK_DOWN_1, AudioManager.MusicId.PUNK_DOWN_7);
        }
    }
}

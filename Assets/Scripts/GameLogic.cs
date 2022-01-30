using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;

    [SerializeField]
    [Range(-100, 100)]
    public int currentCharge = 0;

    public int valueMonk = 41;
    public int valueZen = 91;

    public int FuryPunk = -40;
    public int Monk = 41;
    public int MonkZen = 91;

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
        Debug.Log(currentCharge);
        if (currentCharge >= valueMonk && currentCharge < valueZen)
        {
            Player.Instance.ConvertToMonk();
        }
        if (currentCharge >= valueZen)
        {
            Player.Instance.ConvertToZen();
        }
        if (currentCharge < valueMonk)
        {
            Player.Instance.ConvertToPunk();
        }
    }

    public void PlayMusicByCharge()
    {
        currentCharge = Bar.Instance.CurrentCharge;
        //AudioManager.instance.playMusic() 
    }

}

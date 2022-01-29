using UnityEngine;

public class Bar : MonoBehaviour
{
    public const int CHARGE_MIN = -100;
    public const int CHARGE_MAX = 100;

    // -100 to +100
    public int CurrentCharge;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCharge = 20;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int Charge(int charge)
    {
        CurrentCharge = Mathf.Clamp(CurrentCharge + charge, CHARGE_MIN, CHARGE_MAX);
        return CurrentCharge;
    }
}

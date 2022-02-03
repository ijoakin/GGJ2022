using UnityEngine;

public class Bar : MonoBehaviour
{
    public const int CHARGE_MIN = -100;
    public const int CHARGE_MAX = 100;

    public static Bar Instance;

    // -100 to +100
    public int CurrentCharge;

    public Transform Needle;
    private Vector3 needleInitialPosition;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        needleInitialPosition = Needle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        updateNeedlePosition();
    }

    public int Charge(int charge)
    {
        CurrentCharge = Mathf.Clamp(CurrentCharge + charge, CHARGE_MIN, CHARGE_MAX);
        return CurrentCharge;
    }

    void updateNeedlePosition()
    {
        Vector3 pos = Needle.transform.position;

        // I may explain the multiply factor, but you wouldn't get it... [smoking...]
        pos.x = needleInitialPosition.x + (CurrentCharge / 100.0f) * 150.0f;
        Needle.transform.position = pos;
    }
}

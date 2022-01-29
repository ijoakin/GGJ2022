using UnityEngine;

public class Bar : MonoBehaviour
{
    public static Bar Instance;
    public const int CHARGE_MIN = -100;
    public const int CHARGE_MAX = 100;

    // -100 to +100
    public int CurrentCharge;

    public GameObject Needle;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Needle.transform.position = gameObject.transform.position;
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

        // 0.85 porque coso
        pos.x = gameObject.transform.position.x + (CurrentCharge / 100.0f) * 0.85f;
        Needle.transform.position = pos;
    }
}

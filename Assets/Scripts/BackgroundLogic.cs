using UnityEngine;

public class BackgroundLogic : MonoBehaviour
{
    public GameObject redBg;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float newAlpha = ChargeToAlpha(Bar.Instance.CurrentCharge);
        Color newColor;
        SpriteRenderer sr = redBg.GetComponent<SpriteRenderer>();
        newColor = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
        sr.color = newColor;
    }

    float ChargeToAlpha(float charge)
    {
        return charge >= 0 ? 0.0f : -charge / 100.0f;
    }
}

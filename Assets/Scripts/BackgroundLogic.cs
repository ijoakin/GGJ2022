using UnityEngine;

public class BackgroundLogic : MonoBehaviour
{
    public Bar Bar;
    public ContextAlpha ContextAlpha;

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        float newAlpha = ContextAlpha.ChargeToAlpha(Bar.CurrentCharge);
        Color newColor;
        foreach (SpriteRenderer sr in GameObject.Find("red").gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            newColor = new Color(sr.color.r, sr.color.g, sr.color.b);
            newColor.a = newAlpha;
            sr.color = newColor;
        }       
    }
}

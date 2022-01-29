using UnityEngine;

public class ContextAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ApplyChargeToAlpha(float charge, SpriteRenderer spriteRenderer)
    {
        Color color = spriteRenderer.color;
        color.a = charge >= 0 ? 0.0f : -charge / 100.0f;
        spriteRenderer.color = color;
    }
}

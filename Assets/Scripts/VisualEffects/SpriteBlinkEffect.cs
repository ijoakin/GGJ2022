using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkSpeed;
    public bool playBlinkEffect;

    public bool playBlinkDebug;

    public void PlayBlinkEffect()
    {
        playBlinkEffect = true;
        StartCoroutine(_PlayBlinkEffect());
    }

    public void StopBlinkEffect()
    {
        playBlinkEffect = false;
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator _PlayBlinkEffect()
    {
        float cosValue = 0;
        var sprite = GetComponent<SpriteRenderer>();
        Color currentColor = sprite.color;

        while (playBlinkEffect)
        {
            cosValue = Mathf.Cos(Time.time * blinkSpeed);
            currentColor.a = cosValue <= 0 ? 0 : cosValue;
            sprite.color = currentColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void Update()
    {
        if (playBlinkDebug)
        {
            PlayBlinkEffect();
            playBlinkDebug = false;
        }
    }
}

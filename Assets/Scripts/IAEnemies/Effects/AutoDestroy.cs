using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroyDelay = .2f;
    [SerializeField] private AudioClip destroySfx;

    private void Awake()
    {
        //AudioManager.instance.PlaySfxByAudioClip(destroySfx);
        Destroy(this.gameObject, destroyDelay);
    }
}
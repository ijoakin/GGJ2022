using UnityEngine;

public class TextureOffset : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private string sortingLayer;

    private void Start()
    {
        // this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(sortingLayer);
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
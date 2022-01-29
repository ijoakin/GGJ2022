using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    private Transform target;

    public float minHeight;
    public float maxHeight;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //float clampedY = Mathf.Clamp(target.position.y, minHeight, maxHeight);

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}

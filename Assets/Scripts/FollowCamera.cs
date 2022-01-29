using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target;

    private Transform farBackGround;
    private Transform middleBackGround;

    public float minHeight;
    public float maxHeight;
    private float lastPosX;
    private float lastPosY;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;

        farBackGround = GameObject.Find("back").transform;
        middleBackGround = GameObject.Find("middle").transform;

        lastPosX = transform.position.x;
        lastPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);

        float amountToMoveX = transform.position.x - lastPosX;
        float amountToMoveY = transform.position.y - lastPosY;

        if (amountToMoveX != 0)
        {
            farBackGround.position = farBackGround.position + new Vector3(amountToMoveX, amountToMoveY, 0f);
            middleBackGround.position = middleBackGround.position + new Vector3(amountToMoveX * .5f, amountToMoveY * .5f, 0f);
        }




        lastPosX = transform.position.x;
        lastPosY = transform.position.y;
    }
}

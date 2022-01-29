using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RangeData
{
    public float min;
    public float max;
}

public class CameraController : MonoBehaviour
{
    [Header("Boundary Range")]
    [SerializeField] private RangeData boundaryRangeX;

    [SerializeField] private RangeData boundaryRangeY;

    private RangeData cameraSizeX;
    private RangeData cameraSizeY;

    [SerializeField] private float offsetZ = -20;
    public float offsetX;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private float lastPosGroundY = 0;

    [SerializeField] private Vector2 scaleFactor;
    [SerializeField] private GameObject background;
    [SerializeField] private float sizeSpeed = 2;
    private bool fixCamera;

    private Vector2 fixPosition;

    private void Awake()
    {
        GetCameraSize();
    }

    private void GetCameraSize()
    {
        cameraSizeX = new RangeData();
        cameraSizeY = new RangeData();
        cameraSizeX.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - this.transform.position.x;
        cameraSizeX.max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - this.transform.position.x;
        cameraSizeY.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - this.transform.position.y;
        cameraSizeY.max = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - this.transform.position.y;
    }

    public void FixCamera(float cameraSize, Vector2 pivotCamera)
    {
        fixCamera = true;

        this.fixPosition = pivotCamera;
        StartCoroutine(ChangeCameraSize(cameraSize));
    }

    private IEnumerator ChangeCameraSize(float cameraSize)
    {
        float size = Camera.main.orthographicSize;
        while (size < cameraSize)
        {
            size += sizeSpeed;
            Camera.main.orthographicSize = size;

            background.transform.localScale = new Vector3(size * scaleFactor.x, size * scaleFactor.y, 1);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void UpdateBoundaries(RangeData boundaryX, RangeData boundaryY)
    {
        boundaryRangeX = boundaryX;
        boundaryRangeY = boundaryY;
    }

    private void Update()
    {
        if (fixCamera)
        {
            targetPosition = new Vector3(fixPosition.x, fixPosition.y, offsetZ);
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, 0.3f);
            return;
        }

        //offsetX = Mathf.Sign(Player.Instance.nonZeroMovementX) * 3;

        //if (Player.Instance.playerIsOnGround)
        //{
        //    lastPosGroundY = Player.Instance.transform.position.y;
        //}

        targetPosition = new Vector3(
             Mathf.Clamp(Player.Instance.transform.position.x + offsetX, boundaryRangeX.min - cameraSizeX.min, boundaryRangeX.max - cameraSizeX.max),
             Mathf.Clamp(lastPosGroundY, boundaryRangeY.min - cameraSizeY.min, boundaryRangeY.max - cameraSizeY.max), offsetZ);

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, 0.3f);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        var pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min);
        var pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.min);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.max);
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min);
        pointB = new Vector2(boundaryRangeX.min, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryRangeX.max, boundaryRangeY.min);
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);
    }

#endif
}
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Runtime Assigned")]
    public Transform target;
    public Tilemap tilemap;

    [Header("Movement")]
    public float smoothSpeed = 5f;
    public float deadZoneWidth = 2.5f;

    float fixedY;
    float camHalfWidth;
    float minX;
    float maxX;

    float leftBound;
    float rightBound;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        camHalfWidth = cam.orthographicSize * cam.aspect;
    }

    /// 🔴 每个 Scene 进来都要 call 一次
    public void Setup(Transform newTarget, Tilemap newTilemap, float lockY)
    {
        target = newTarget;
        tilemap = newTilemap;
        fixedY = lockY;

        tilemap.CompressBounds();
        Bounds b = tilemap.localBounds;

        minX = b.min.x + camHalfWidth;
        maxX = b.max.x - camHalfWidth;

        leftBound  = transform.position.x - deadZoneWidth;
        rightBound = transform.position.x + deadZoneWidth;
    }

    void LateUpdate()
    {
        if (target == null || tilemap == null) return;

        Vector3 camPos = transform.position;

        if (target.position.x > rightBound)
            camPos.x = target.position.x - deadZoneWidth;
        else if (target.position.x < leftBound)
            camPos.x = target.position.x + deadZoneWidth;

        camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
        camPos.y = fixedY;

        transform.position = Vector3.Lerp(
            transform.position,
            camPos,
            smoothSpeed * Time.deltaTime
        );

        leftBound  = transform.position.x - deadZoneWidth;
        rightBound = transform.position.x + deadZoneWidth;
    }
}

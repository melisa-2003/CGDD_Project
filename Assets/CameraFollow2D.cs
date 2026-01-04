using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Runtime Assigned")]
    public Transform target;        // player to follow
    public Tilemap tilemap;         // current level tilemap

    [Header("Movement Settings")]
    public float smoothSpeed = 5f;
    public float deadZoneWidth = 2.5f;

    private float fixedY;
    private float camHalfWidth;
    private float minX, maxX;
    private float leftBound, rightBound;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        camHalfWidth = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect;
    }

    void Update()
    {
        // Keep searching for the player if not assigned yet
        if (target == null)
        {
            FindPlayerAndTilemap();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assign player and tilemap immediately after scene load
        FindPlayerAndTilemap();

        // Snap camera to player start position
        if (target != null && tilemap != null)
        {
            Vector3 camPos = transform.position;
            camPos.x = Mathf.Clamp(target.position.x, minX, maxX);
            camPos.y = fixedY; // or target.position.y if you want free vertical
            transform.position = camPos;

            // Update dead zones after snapping
            leftBound = transform.position.x - deadZoneWidth;
            rightBound = transform.position.x + deadZoneWidth;
        }
    }

    private void FindPlayerAndTilemap()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        Tilemap map = Object.FindFirstObjectByType<Tilemap>();

        if (playerObj != null && map != null)
        {
            float cameraY = transform.position.y; // keep camera height consistent
            Setup(playerObj.transform, map, cameraY);

        }
    }

    public void Setup(Transform newTarget, Tilemap newTilemap, float lockY)
    {
        target = newTarget;
        tilemap = newTilemap;
        fixedY = lockY;

        tilemap.CompressBounds();
        Bounds b = tilemap.localBounds;
        minX = b.min.x + camHalfWidth;
        maxX = b.max.x - camHalfWidth;

        leftBound = transform.position.x - deadZoneWidth;
        rightBound = transform.position.x + deadZoneWidth;
    }

    void LateUpdate()
    {
        if (target == null || tilemap == null) return;

        Vector3 camPos = transform.position;

        // Horizontal dead zone
        if (target.position.x > rightBound)
            camPos.x = target.position.x - deadZoneWidth;
        else if (target.position.x < leftBound)
            camPos.x = target.position.x + deadZoneWidth;

        camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
        camPos.y = fixedY;

        transform.position = Vector3.Lerp(transform.position, camPos, smoothSpeed * Time.deltaTime);

        leftBound = transform.position.x - deadZoneWidth;
        rightBound = transform.position.x + deadZoneWidth;
    }
}

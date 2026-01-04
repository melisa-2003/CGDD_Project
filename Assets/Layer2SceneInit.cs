using UnityEngine;
using UnityEngine.Tilemaps;

public class Layer2SceneInit : MonoBehaviour
{
    [Header("Scene References")]
    public CameraFollow2D cameraFollow;
    public Transform player;
    public Tilemap groundTilemap;

    [Header("Camera Settings")]
    public float cameraLockY = 0f;

    void Start()
    {
        if (cameraFollow == null)
        {
            Debug.LogError("[Layer2SceneInit] CameraFollow2D not assigned!");
            return;
        }

        cameraFollow.Setup(player, groundTilemap, cameraLockY);
    }
}
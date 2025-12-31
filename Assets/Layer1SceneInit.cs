using UnityEngine;
using UnityEngine.Tilemaps;

public class Layer1SceneInit : MonoBehaviour
{
    public Transform player;
    public Tilemap groundTilemap;
    public float cameraLockY = 0f;

    void Start()
    {
        CameraFollow2D cam =
            Camera.main.GetComponent<CameraFollow2D>();

        cam.Setup(player, groundTilemap, cameraLockY);
    }
}


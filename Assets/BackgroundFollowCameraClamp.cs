using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    public Transform cam;
    public float fixedY;
    public float fixedZ;

    void LateUpdate()
    {
        if (!cam) return;

        transform.position = new Vector3(
            cam.position.x,
            fixedY,
            fixedZ
        );
    }
}

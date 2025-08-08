using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;             // Camera theo dõi
    public float parallaxFactorX = 0.5f; // Tốc độ parallax trục X
    public float parallaxFactorY = 0f;   // Tốc độ parallax trục Y (0 = không dịch theo Y)

    private Vector3 lastCamPosition;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        lastCamPosition = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPosition;

        transform.position += new Vector3(delta.x * parallaxFactorX, delta.y * parallaxFactorY, 0);

        lastCamPosition = cam.position;
    }
}

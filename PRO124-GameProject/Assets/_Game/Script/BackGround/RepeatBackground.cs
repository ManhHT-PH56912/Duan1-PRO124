using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    public float length; // Chiều dài ảnh nền
    public Transform cam;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        if (cam == null)
            cam = Camera.main.transform;
    }

    void Update()
    {
        float temp = cam.position.x * (1 - 1f); // 1f = parallaxFactor bạn đang dùng nếu cần

        float distance = cam.position.x * 1f;

        if (distance > startPosition.x + length)
        {
            startPosition.x += length;
        }
        else if (distance < startPosition.x - length)
        {
            startPosition.x -= length;
        }

        transform.position = new Vector3(startPosition.x, transform.position.y, transform.position.z);
    }
}

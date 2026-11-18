using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoFit : MonoBehaviour
{

    private Camera cam;
    public float targetAspect = 16f / 9f; // Tỷ lệ thiết kế chuẩn của bạn

    void Start()
    {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        if (currentAspect < targetAspect)
        {
            // Màn hình hẹp hơn tỷ lệ chuẩn (như điện thoại dài 20:9), mở rộng camera theo chiều dọc
            cam.orthographicSize = cam.orthographicSize * (targetAspect / currentAspect);
        }
    }
}



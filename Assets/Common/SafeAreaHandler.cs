using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // Tự động cập nhật lại nếu người chơi xoay màn hình hoặc thay đổi kích thước
        if (Screen.safeArea != lastSafeArea)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Chuyển đổi tọa độ pixel của SafeArea sang tỷ lệ chuẩn từ 0 đến 1
        Vector2 minAnchor = safeArea.min;
        Vector2 maxAnchor = safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        // Gán lại Anchor cho RectTransform
        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;

        // Đưa khoảng cách lề (offset) về 0 để khớp hoàn toàn với vùng an toàn
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        lastSafeArea = safeArea;
    }
}
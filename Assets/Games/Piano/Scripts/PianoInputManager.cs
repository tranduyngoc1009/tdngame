using UnityEngine;
using Lean.Touch;

public class PianoInputManager : MonoBehaviour
{
    void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        // Chuyển vị trí ngón tay chạm trên màn hình sang tọa độ World Space
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
        
        // Quét tất cả các Collider tại điểm chạm
        Collider2D[] hits = Physics2D.OverlapPointAll(rayPos);

        if (hits.Length > 0)
        {
            GameObject targetKey = null;
            int highestSortingOrder = int.MinValue;

            // Thuật toán Raycast Filter: Tìm đối tượng có Sorting Order cao nhất (Phím đen nằm ở trên phím trắng)
            foreach (var hit in hits)
            {
                SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (sr.sortingOrder > highestSortingOrder)
                    {
                        highestSortingOrder = sr.sortingOrder;
                        targetKey = hit.gameObject;
                    }
                }
            }

            // Kích hoạt phím chính xác dựa vào tên class tương ứng
            if (targetKey != null)
            {
                // Kiểm tra nếu là phím trắng
                PianoKey whiteKey = targetKey.GetComponent<PianoKey>();
                if (whiteKey != null)
                {
                    whiteKey.TriggerKeyAction();
                    return;
                }

                // Kiểm tra nếu là phím đen
                BlackPianoKey blackKey = targetKey.GetComponent<BlackPianoKey>();
                if (blackKey != null)
                {
                    blackKey.TriggerKeyAction();
                    return;
                }
            }
        }
    }
}
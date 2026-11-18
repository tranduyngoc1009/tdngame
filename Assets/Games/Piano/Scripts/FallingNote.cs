using System;
using UnityEngine;

public class FallingNote : MonoBehaviour
{
     public int keyIndex;
    private float speed;
    private bool isInitialized = false;

    public void Initialize(int keyIdx, float fallSpeed)
    {
        keyIndex = keyIdx;
        speed = fallSpeed;
        isInitialized = true;
    }

   
    void Update()
    {
        if (!isInitialized) return;

        // Cho nốt rơi xuống
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    // TRƯỜNG HỢP 1: Nốt rơi tự do chạm vào vùng phím bấm ở đáy màn hình
    private void OnTriggerEnter2D(Collider2D other) // Nếu dùng 3D thì đổi thành OnTriggerEnter(Collider other)
    {
        
        // Kiểm tra xem có chạm vào đúng phím đàn tương ứng hoặc vạch đích không
        // (Bạn có thể check tag "PianoKey" hoặc kiểm tra script phím)
        if ( other.GetComponent<PianoKey>() != null)
        {
            // Kiểm tra xem phím bị chạm có đúng là làn của nốt này không (nếu cần thiết)
            PianoKey key = other.GetComponent<PianoKey>();
     
          HitNote();

            if (key != null && key.keyIndex == this.keyIndex)
            {
                // HitNote();
            }
        }
    }

    // TRƯỜNG HỢP 2: Người chơi bấm phím đúng lúc nốt đang ở vùng phím
    public void HitNote()
    {
        // // Báo cho SongManager biết nốt này đã biến mất sớm/đúng hạn để thả nốt tiếp theo
        // if (SongManager.Instance != null)
        // {
        //     SongManager.Instance.OnNoteDestroyedEarly();
        // }

        // Hủy đối tượng nốt nhạc
        //SongManager.Instance.listSpawn.Remove(gameObject.GetComponent<FallingNote>());
        Destroy(gameObject);
        
    }
}

    // void OnMiss()
    // {
    //     Debug.Log($"Miss! Bỏ lỡ nốt ở phím {keyIndex}");
    //     // Xóa nốt khỏi hàng đợi của phím tương ứng
    //     RemoveFromKeyLane();
    //     Destroy(gameObject);
    // }
    //
    // // Được gọi khi người chơi bấm đúng phím kịp thời
    // public void ProcessHit()
    // {
    //     if (isProcessed) return;
    //     isProcessed = true;
    //
    //     Debug.Log($"Hit chính xác nốt ở phím {keyIndex}!");
    //     RemoveFromKeyLane();
    //     
    //     // (Tùy chọn) Sinh hiệu ứng Particle System báo điểm Perfect ở đây
    //     Destroy(gameObject);
    // }
    //
    // void RemoveFromKeyLane()
    // {
    //     // Gỡ nốt này ra khỏi danh sách chờ của phím
    //     if (targetKeyScript is PianoKey whiteKey)
    //     {
    //         // whiteKey.RemoveNoteFromLane(this); // Nếu bạn có viết hàm remove riêng
    //     }
    //     else if (targetKeyScript is BlackPianoKey blackKey)
    //     {
    //         // blackKey.RemoveNoteFromLane(this);
    //     }
    // }

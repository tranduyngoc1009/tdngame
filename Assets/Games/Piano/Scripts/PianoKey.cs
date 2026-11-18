using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class PianoKey : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    
    [Header("Cài đặt hiệu ứng lún")]
    public float scaleDownY = 0.85f;
    public float duration = 0.08f;

    [Header("Khóa phím chống Spam")]
    private bool isPlayingAnimation = false;

    [Header("Màu sắc")]
    private Color normalColor;
    private Color pressedColor;
    
    [Header("Chế độ học tập - Hàng đợi nốt")]
    private List<FallingNote> activeNotesInLane = new List<FallingNote>();

    public int keyIndex;
    // Chỉ số làn của phím này (Ví dụ: phím số 0, 1, 2...)
    public int laneIndex;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        GenerateRandomColors();
    }

    void GenerateRandomColors()
    {
        // Tự động random màu sắc tươi tắn cho phím trắng
        normalColor = Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.8f, 1.0f);
        spriteRenderer.color = normalColor;

        // Tạo phiên bản màu đậm hơn khi ấn phím
        pressedColor = new Color(normalColor.r * 0.6f, normalColor.g * 0.6f, normalColor.b * 0.6f);
    }

    public void TriggerKeyAction()
    {
        // Nếu phím đang bận chạy animation thì bỏ qua (chống spam)
        if (isPlayingAnimation) return;
        isPlayingAnimation = true;

        // 1. Phát âm thanh ngay lập tức
        if (audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // 2. Chạy hiệu ứng lún mượt mà bằng DOTween Sequence
        transform.DOKill();
        spriteRenderer.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(originalScale.y * scaleDownY, duration * 1.0f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScaleY(originalScale.y, duration * 1.5f).SetEase(Ease.OutBack));

        // Đổi màu đậm khi bấm và hồi lại màu cũ
        spriteRenderer.DOColor(pressedColor, duration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);

        // Khi chạy xong animation thì mở khóa phím
        seq.OnComplete(() => { isPlayingAnimation = false; });
        // ==========================================
        // 3. LOGIC XỬ LÝ ĂN NỐT (LEARNING MODE HIT)
        //
        // FallingNote targetNote = activeNotesInLane[0];// Làm sạch các nốt đã mất
        // targetNote.HitNote();
      //  Debug.Log(activeNotesInLane.Count+ " actinote");
      
        // check xóa nốt rơi khi bấm dúng phím
        
       SongManager.Instance.CheckPressKey(keyIndex);
      
    }
    // Hàm được gọi khi người chơi bấm vào phím này
    // public void OnKeyPressed()
    // {
    //     // Tìm tất cả các nốt đang rơi trên màn hình
    //     FallingNote[] allNotes = FindObjectsByType<FallingNote>(FindObjectsSortMode.None);
    //
    //     foreach (var note in allNotes)
    //     {
    //         // Nếu nốt này nằm cùng làn với phím đang được bấm và đang ở khoảng cách có thể bấm (gần vạch đích)
    //         if (note != null && note.keyIndex == this.keyIndex)
    //         {
    //             // Kiểm tra khoảng cách hoặc cho phép bấm luôn nốt thấp nhất ở làn này
    //             note.HitNote(); // Gọi hàm hủy nốt và kích hoạt nốt tiếp theo
    //             break;
    //         }
    //     }
    // }

// Hàm nhận nốt mới rơi vào cột
  
    public void AddNoteToLane(FallingNote note)
    {
        if (!activeNotesInLane.Contains(note))
        {
            activeNotesInLane.Add(note);
        }
    }
    // HÀM BẠN ĐANG THIẾU: Trả về danh sách nốt hiện tại để SongManager kiểm tra khoảng cách
    public List<FallingNote> GetActiveNotesInLane()
    {
        // Dọn dẹp các nốt đã bị hủy (null) trước khi trả về
        activeNotesInLane.RemoveAll(note => note == null);
        return activeNotesInLane;
    }

    public void RemoveNoteFromLane(FallingNote note)
    {
        if (activeNotesInLane.Contains(note))
        {
            activeNotesInLane.Remove(note);
        }
    }
    public void ClearLane() { activeNotesInLane.Clear(); }

}
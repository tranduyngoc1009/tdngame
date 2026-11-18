using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class BlackPianoKey : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    
    [Header("Cài đặt màu sắc phím đen")]
    [Tooltip("Màu sắc mặc định của phím đen khi dùng sprite trắng (Ví dụ: Màu đen tuyền hoặc xám đậm)")]
    public Color defaultBlackColor = new Color(0.15f, 0.15f, 0.15f, 1f); 
    
    [Tooltip("Màu sắc khi phím bị ấn xuống (Tối hơn màu mặc định một chút)")]
    public Color pressedColor = new Color(0.05f, 0.05f, 0.05f, 1f);

    [Header("Cài đặt hiệu ứng lún")]
    public float scaleDownY = 0.85f;
    public float duration = 0.08f;

    [Header("Khóa phím chống Spam")]
    private bool isPlayingAnimation = false;
    private List<FallingNote> activeNotesInLane = new List<FallingNote>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        
        // **THIẾT LẬP MÀU ĐEN CHO SPRITE TRẮNG NGAY TỪ ĐẦU**
        //spriteRenderer.color = defaultBlackColor;
    }

    public void TriggerKeyAction()
    {
        if (isPlayingAnimation) return;
        isPlayingAnimation = true;

        // 1. Phát âm thanh nốt thăng/giáng
        if (audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // 2. Chạy hiệu ứng lún mượt mà bằng DOTween Sequence
        transform.DOKill();
        spriteRenderer.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(originalScale.y * scaleDownY, duration * 0.7f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScaleY(originalScale.y, duration * 1.3f).SetEase(Ease.OutBack));

        // Đổi sang màu tối hơn khi bấm và hồi lại màu đen mặc định
        spriteRenderer.DOColor(pressedColor, duration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);

        seq.OnComplete(() => {
            isPlayingAnimation = false;
        });
        
        // 3. LOGIC XỬ LÝ ĂN NỐT (LEARNING MODE HIT)
        activeNotesInLane.RemoveAll(n => n == null); // Làm sạch các nốt đã mất
        if (activeNotesInLane.Count > 0)
        {
            // Lấy nốt đang ở đầu hàng đợi (nốt xuất hiện sớm nhất trên cột)
            FallingNote targetNote = activeNotesInLane[0];
            targetNote.HitNote(); // Thực hiện hành động ăn nốt (Hủy nốt, cộng điểm)
            activeNotesInLane.RemoveAt(0);
        }
    }
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
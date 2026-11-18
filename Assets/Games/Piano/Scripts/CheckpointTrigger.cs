using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // Dùng OnTriggerEnter nếu là game 3D
    {
        // Kiểm tra xem đối tượng va chạm có phải là Nốt rơi không
        FallingNote note = other.GetComponent<FallingNote>();
        if (note != null)
        {
            // Báo cho SongManager biết nốt này đã vượt qua vạch an toàn
            if (SongManager.Instance != null)
            {
                SongManager.Instance.OnNoteReachedCheckpoint(note);
            }
        }
    }
}
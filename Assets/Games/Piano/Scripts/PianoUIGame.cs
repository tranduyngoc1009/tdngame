using System;
using UnityEngine;
using UnityEngine.UI;

public class PianoUIGame : MonoBehaviour
{
    public GameObject popupLevel;

    public Button buttonPopup;
    private bool isPaused = false;

    private void Start()
    {
        popupLevel.SetActive(false);
    }

    public void OpenPopup()
    {
       if (isPaused)
       {
           ResumeGame();
       }
       else
       {
           PauseGame();
       }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Đóng băng toàn bộ thời gian trong game (nốt nhạc dừng sững lại)

        if (popupLevel != null)
        {
            popupLevel.SetActive(true); // Hiện bảng Pause lên
        }

        // Nếu game có phát nhạc nền (AudioSource), hãy tạm dừng nhạc ở đây:
        // AudioSource backgroundMusic = FindObjectOfType<AudioSource>();
        // if (backgroundMusic != null) backgroundMusic.Pause();

        Debug.Log("Game đã tạm dừng!");
    }

    // --- HÀM BẤM NÚT TIẾP TỤC (Gắn vào nút Play/Tiếp tục trên bảng Pause) ---
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Khôi phục lại thời gian bình thường

        if (popupLevel != null)
        {
            popupLevel.SetActive(false); // Tắt bảng Pause đi
        }

        // Tiếp tục phát nhạc nền:
        // AudioSource backgroundMusic = FindObjectOfType<AudioSource>();
        // if (backgroundMusic != null) backgroundMusic.UnPause();

        Debug.Log("Game tiếp tục chạy!");
    }

    // --- HÀM CHƠI LẠI (Gắn vào nút Replay/Chơi lại) ---
    public void RestartGame()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian phải trở về 1 trước khi load lại
        
        // Gọi lại hàm phát bài hát hiện tại từ SongManager
        if (SongManager.Instance != null)
        {
            // Ví dụ load lại bài hiện tại (bạn có thể lưu index bài hát đang chơi)
            SongManager.Instance.PlaySong(0); 
        }

        if (popupLevel != null) popupLevel.SetActive(false);
        isPaused = false;
    }

    // --- HÀM VỀ MENU CHÍNH (Gắn vào nút Home) ---
    public void BackToMenu()
    {
        Time.timeScale = 1f; // Đưa thời gian về bình thường
        // Chuyển Scene về Menu chính của bạn
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
    public void ClosePopup()
    {
        popupLevel.SetActive(false);
    }
    // Gắn hàm này vào các nút bấm chọn độ khó trên UI
    public void OnSelectDifficulty(int difficultyIndex)
    {
        float selectedSpeed = 5f; // Mặc định Medium

        switch (difficultyIndex)
        {
            case 0: selectedSpeed = 1f; break;   // Easy
            case 1: selectedSpeed = 1.4f; break;   // Medium
            case 2: selectedSpeed = 1.8f; break; // Hard
            case 3: selectedSpeed = 2f; break;  // Expert
        }

        // Ví dụ phát bài hát đầu tiên với tốc độ đã chọn
        if (SongManager.Instance != null && SongManager.Instance.allSongs.Count > 0)
        {
            // Lấy bài hát hiện tại và truyền tốc độ mới vào
            SongDataModel currentSelectedSong = SongManager.Instance.allSongs[0]; 
            SongManager.Instance.StartSong(currentSelectedSong, selectedSpeed);
        }
        ResumeGame();
    }

    
}

using DG.Tweening;
using UnityEngine;

public enum GameMode
{
    Menu,       // Màn hình chọn bài / Chọn chế độ
    Learning,   // Chế độ học tập (Falling Notes)
    FreePlay    // Chế độ tự do (Bấm phím phát nhạc không cần nốt rơi)
}

public class PianoGameManager : MonoBehaviour
{
    public static PianoGameManager Instance;

    [Header("Trạng thái hiện tại")]
    public GameMode currentMode = GameMode.FreePlay;

    [Header("UI Panels")]
    public GameObject menuPanel;        // Panel giao diện chọn bài
    public GameObject gameplayHUD;
    public int indexsong = 0;// Giao diện trong lúc chơi (Nút dừng, điểm số...)

    void Awake()
    {
        // Singleton pattern để gọi GameManager từ bất cứ đâu
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển Scene (nếu có)
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Khởi động game ở trạng thái Menu
       SetGameMode(currentMode);

       // DOVirtual.DelayedCall(5f, () => {
       //     Debug.Log("Đã qua 1.5 giây bằng DOTween!");
       //     StartLearningSong(indexsong);
       // });
      
    }

    // Hàm chuyển đổi các chế độ của game
    public void SetGameMode(GameMode newMode)
    {
        currentMode = newMode;

        switch (currentMode)
        {
            case GameMode.Menu:
                if (menuPanel) menuPanel.SetActive(true);
                if (gameplayHUD) gameplayHUD.SetActive(false);
                
                // Dừng bài hát hiện tại nếu đang chơi
                if (SongManager.Instance != null)
                {
                    //SongManager.Instance.StopAndResetSong();
                }
                break;

            case GameMode.Learning:
               // if (menuPanel) menuPanel.SetActive(false);
                if (gameplayHUD) gameplayHUD.SetActive(true);
                break;

            case GameMode.FreePlay:
                if (menuPanel) menuPanel.SetActive(false);
                if (gameplayHUD) gameplayHUD.SetActive(true);
                
                // Dừng hệ thống nốt rơi, chỉ cho phép bấm phím tự do
                if (SongManager.Instance != null)
                {
                    //SongManager.Instance.StopAndResetSong();
                }
                break;
        }

        Debug.Log($"Đã chuyển sang chế độ: {currentMode}");
    }

    // Hàm gọi khi người chơi bấm chọn một bài hát từ Menu để bắt đầu học tập
    public void StartLearningSong(int songIndex)
    {
        SetGameMode(GameMode.Learning);

        if (SongManager.Instance != null)
        {
            //SongManager.Instance.PlaySong(songIndex);
        }
    }

    // Hàm quay lại Menu chính từ trong màn chơi
    public void ReturnToMenu()
    {
        SetGameMode(GameMode.Menu);
    }
}


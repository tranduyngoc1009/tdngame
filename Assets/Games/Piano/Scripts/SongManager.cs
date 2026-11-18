using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Thư viện DOTween (nếu bạn đang dùng)

public enum GameDifficulty
{
    Easy = 3, // Tốc độ chậm cho người mới
    Medium = 5, // Tốc độ tiêu chuẩn
    Hard = 7, // Tốc độ nhanh, thử thách phản xạ
    Expert = 10 // Siêu tốc độ dành cho cao thủ
}

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    [Header("Google Sheet API")] [Tooltip("Dán URL Web App của Google Apps Script vào đây")]
    public string sheetApiUrl = "";

    [Header("Cài đặt Game")] public float fallSpeed = 3f;
    public Transform[] spawnPoints;
    public GameObject notePrefab;

    [HideInInspector] public List<SongDataModel> allSongs = new List<SongDataModel>();
    [HideInInspector] public bool isDataLoaded = false;

    private SongDataModel currentSong;
    private int currentNoteIndex = 0;
    private bool canSpawnNext = true;

    public List<FallingNote> listSpawn = new List<FallingNote>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Tự động tải dữ liệu từ Google Sheet khi khởi động (GIỮ NGUYÊN CƠ CHẾ CŨ)
        if (!string.IsNullOrEmpty(sheetApiUrl))
        {
            StartCoroutine(FetchSongsFromGoogleSheet());
        }
        else
        {
            Debug.LogError("Chưa cấu hình Sheet API URL cho SongManager!");
        }
    }

    // --- COROUTINE TẢI DỮ LIỆU TỪ GOOGLE SHEET (GIỮ NGUYÊN) ---
    IEnumerator FetchSongsFromGoogleSheet()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(sheetApiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi tải bài hát từ Google Sheet: " + www.error);
            }
            else
            {
                string jsonResult = www.downloadHandler.text;
                string wrappedJson = "{\"songs\":" + jsonResult + "}";

                SongListWrapper wrapper = JsonUtility.FromJson<SongListWrapper>(wrappedJson);
                if (wrapper != null && wrapper.songs != null)
                {
                    allSongs = new List<SongDataModel>(wrapper.songs);
                    isDataLoaded = true;
                    Debug.Log($"<color=green>Đã tải thành công {allSongs.Count} bài hát từ Google Sheet!</color>");

                    // TỰ ĐỘNG PHÁT LUÔN BÀI ĐẦU TIÊN KHI TẢI XONG
                    if (allSongs.Count > 0)
                    {
                        PlaySong(0);
                    }
                }
            }
        }
    }

    // --- HÀM PHÁT BÀI HÁT (ĐỂ TƯƠNG THÍCH VỚI GAMEMANAGER VÀ INSPECTOR) ---
    public void PlaySong(int songIndex)
    {
        if (!isDataLoaded || allSongs == null || allSongs.Count == 0)
        {
            Debug.LogWarning("Dữ liệu từ Google Sheet đang được tải, đang đợi để phát bài...");
            DOVirtual.DelayedCall(1.0f, () => { PlaySong(songIndex); });
            return;
        }

        if (songIndex < 0 || songIndex >= allSongs.Count) return;

        StartSong(allSongs[songIndex]);
    }

    public void StartSong(SongDataModel song, float customFallSpeed = -1f)
    {
        if (song == null || song.notes == null || song.notes.Length == 0) return;

        // Nếu có truyền tốc độ riêng cho level đó thì cập nhật
        if (customFallSpeed > 0)
        {
            fallSpeed = customFallSpeed;
        }

        currentSong = song;
        currentNoteIndex = 0;
        canSpawnNext = true;

        ClearAllActiveNotes();

        // Spawn ngay nốt đầu tiên bắt đầu bài hát
        SpawnNextNote();
        Debug.Log($"Đang phát bài: {song.songTitle}");
    }

    // --- HÀM SPAWN NỐT TIẾP THEO ---
    void SpawnNextNote()
    {
        if (currentSong == null || currentNoteIndex >= currentSong.notes.Length) return;
        if (!canSpawnNext) return;


        NoteJsonData targetNote = currentSong.notes[currentNoteIndex];

        int keyIndex = targetNote.keyIndex;
        if (keyIndex >= 0 && keyIndex < spawnPoints.Length)
        {
            GameObject noteObj = Instantiate(notePrefab, spawnPoints[keyIndex].position, Quaternion.identity);
            FallingNote fallingNote = noteObj.GetComponent<FallingNote>();
            if (fallingNote != null)
            {
                fallingNote.Initialize(keyIndex, fallSpeed);
                listSpawn.Add(fallingNote);
            }

            canSpawnNext = false;
            currentNoteIndex++;
        }
    }

    private FallingNote firstFallNote;
    public void CheckPressKey(int keyIndex)
    {
        if (listSpawn.Count > 0 && listSpawn[0].keyIndex == keyIndex)
        {
            for (int i = 0; i < listSpawn.Count; i++)
            {
                if (listSpawn[i] != null)
                {
                    firstFallNote = listSpawn[i];
                    listSpawn.Remove(listSpawn[i]);
                    if (listSpawn.Count == 0)
                    {
                        OnNoteDestroyedEarly();
                    }
                    firstFallNote.HitNote();
                }
            }

            Debug.Log("yehhhhhhh");
        }
        else
        {
            Debug.Log("bạn đã bấm sai nốt");
        }
    }

    // --- NHẬN TÍN HIỆU TỪ VẠCH CHECKPOINT ---
    public void OnNoteReachedCheckpoint(FallingNote note)
    {
        canSpawnNext = true;
        SpawnNextNote();
    }

    // --- NHẬN TÍN HIỆU KHI NỐT BỊ BẤM MẤT TRƯỚC ---
    public void OnNoteDestroyedEarly()
    {
        canSpawnNext = true;
        SpawnNextNote();
    }

    void ClearAllActiveNotes()
    {
        foreach (var n in listSpawn)
        {
            if (n != null) Destroy(n.gameObject);
        }
    }

    // Các hàm bổ trợ
    public int GetTotalSongsCount() => allSongs != null ? allSongs.Count : 0;
    public string GetCurrentSongTitle() => currentSong != null ? currentSong.songTitle : "Chưa chọn bài nào";
}
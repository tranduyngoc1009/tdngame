using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiSongAssetGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Piano/Tạo Bài Hát/Tạo 3 Bài Hát Thiếu Nhi")]
    public static void CreateAllKidsSongs()
    {
        string dirPath = "Assets/Games/Piano/Resources/Songs";
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder(dirPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Songs");
        }

        // --- BÀI 1: MỘT CON VỊT ---
        SongData song1 = ScriptableObject.CreateInstance<SongData>();
        song1.songTitle = "Một Con Vịt";
        song1.bpm = 120f;
        song1.notes = new NoteData[]
        {
            new NoteData { timeStamp = 1.0f, keyIndex = 2 },
            new NoteData { timeStamp = 1.5f, keyIndex = 4 },
            new NoteData { timeStamp = 2.0f, keyIndex = 5 },
            new NoteData { timeStamp = 2.5f, keyIndex = 7 },
            new NoteData { timeStamp = 3.2f, keyIndex = 7 },
            new NoteData { timeStamp = 4.0f, keyIndex = 9 },
            new NoteData { timeStamp = 4.5f, keyIndex = 9 },
            new NoteData { timeStamp = 5.0f, keyIndex = 9 },
            new NoteData { timeStamp = 5.5f, keyIndex = 7 },
            new NoteData { timeStamp = 6.5f, keyIndex = 9 },
            new NoteData { timeStamp = 7.0f, keyIndex = 9 },
            new NoteData { timeStamp = 7.5f, keyIndex = 7 },
            new NoteData { timeStamp = 8.0f, keyIndex = 5 },
            new NoteData { timeStamp = 8.5f, keyIndex = 4 },
            new NoteData { timeStamp = 9.5f, keyIndex = 2 }
        };
        AssetDatabase.CreateAsset(song1, $"{dirPath}/Song_MotConVit.asset");

        // --- BÀI 2: KÌ KIA CON BƯỚM VÀNG ---
        SongData song2 = ScriptableObject.CreateInstance<SongData>();
        song2.songTitle = "Kì Kia Con Bướm Vàng";
        song2.bpm = 110f;
        song2.notes = new NoteData[]
        {
            new NoteData { timeStamp = 1.0f, keyIndex = 2 },
            new NoteData { timeStamp = 1.5f, keyIndex = 4 },
            new NoteData { timeStamp = 2.0f, keyIndex = 5 },
            new NoteData { timeStamp = 3.0f, keyIndex = 2 },
            new NoteData { timeStamp = 3.5f, keyIndex = 4 },
            new NoteData { timeStamp = 4.0f, keyIndex = 5 },
            new NoteData { timeStamp = 5.0f, keyIndex = 5 },
            new NoteData { timeStamp = 5.4f, keyIndex = 7 },
            new NoteData { timeStamp = 5.8f, keyIndex = 9 },
            new NoteData { timeStamp = 6.5f, keyIndex = 5 },
            new NoteData { timeStamp = 6.9f, keyIndex = 7 },
            new NoteData { timeStamp = 7.3f, keyIndex = 9 }
        };
        AssetDatabase.CreateAsset(song2, $"{dirPath}/Song_KiKiaConBuomVang.asset");

        // --- BÀI 3: CẢ NHÀ THƯƠNG NHAU ---
        SongData song3 = ScriptableObject.CreateInstance<SongData>();
        song3.songTitle = "Cả Nhà Thương Nhau";
        song3.bpm = 100f;
        song3.notes = new NoteData[]
        {
            new NoteData { timeStamp = 1.0f, keyIndex = 4 },
            new NoteData { timeStamp = 1.6f, keyIndex = 5 },
            new NoteData { timeStamp = 2.2f, keyIndex = 7 },
            new NoteData { timeStamp = 3.2f, keyIndex = 7 },
            new NoteData { timeStamp = 4.0f, keyIndex = 5 },
            new NoteData { timeStamp = 4.6f, keyIndex = 4 },
            new NoteData { timeStamp = 5.2f, keyIndex = 2 },
            new NoteData { timeStamp = 6.2f, keyIndex = 2 },
            new NoteData { timeStamp = 7.0f, keyIndex = 4 },
            new NoteData { timeStamp = 7.6f, keyIndex = 2 }
        };
        AssetDatabase.CreateAsset(song3, $"{dirPath}/Song_CaNhaThuongNhau.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("<color=green>Đã tự động tạo thành công 3 bài hát thiếu nhi trong thư mục Resources/Songs!</color>");
    }
#endif
}
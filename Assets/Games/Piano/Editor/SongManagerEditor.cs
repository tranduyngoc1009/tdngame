#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SongManager))]
public class SongManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Vẽ lại giao diện Inspector mặc định của SongManager
        DrawDefaultInspector();

        SongManager songManager = (SongManager)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("--- QUẢN LÝ TRẠNG THÁI BÀI HÁT (EDITOR DEBUG) ---", EditorStyles.boldLabel);

        if (songManager.allSongs == null || songManager.allSongs.Count == 0)
        {
            EditorGUILayout.HelpBox("Chưa tải được bài hát hoặc danh sách đang trống (Hãy bấm Play để tải từ Google Sheet).", MessageType.Warning);
            return;
        }

        // Hiển thị tổng số bài hát
        EditorGUILayout.TextField("Tổng số bài hát:", songManager.allSongs.Count.ToString());

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Danh sách bài hát hiện tại từ Google Sheet:", EditorStyles.boldLabel);

        // Hiển thị danh sách chi tiết Index và Tên bài hát dưới dạng bảng thông tin trực quan
        for (int i = 0; i < songManager.allSongs.Count; i++)
        {
            var song = songManager.allSongs[i];
            
            EditorGUILayout.BeginHorizontal("box");
            
            // Hiển thị Index và Tên bài
            EditorGUILayout.LabelField($"[Index: {i}]", GUILayout.Width(70));
            EditorGUILayout.LabelField($"{song.songTitle} (BPM: {song.bpm})");

            // Nút bấm thử nhanh bài hát ngay trên Editor khi đang Play game
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Play Bài Này", GUILayout.Width(90)))
                {
                    songManager.PlaySong(i);
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }

        // Cập nhật lại giao diện khi có thay đổi
        if (GUI.changed)
        {
            EditorUtility.SetDirty(songManager);
        }
    }
}
#endif
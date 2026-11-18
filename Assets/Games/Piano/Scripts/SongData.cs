using UnityEngine;

[System.Serializable]
public struct NoteData
{
    public float timeStamp;    // Thời gian nốt xuất hiện (tính bằng giây từ lúc bắt đầu bài)
    public int keyIndex;       // Số thứ tự phím (0 đến 13)
}

[CreateAssetMenu(fileName = "NewSong", menuName = "Piano/Song Data")]
public class SongData : ScriptableObject
{
    public string songTitle;
    public AudioClip songAudioClip; // Nhạc nền bài hát
    public float bpm = 120f;
    public NoteData[] notes;        // Danh sách các nốt trong bài
}
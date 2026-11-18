using System;
using System.Collections.Generic;

[Serializable]
public class NoteJsonData
{
    public float timeStamp;    // Thời gian nốt rơi (tính bằng giây)
    public int keyIndex;       // Chỉ số phím đàn (0 đến 13)
}

[Serializable]
public class SongDataModel
{
    public string songTitle;           // Tên bài hát
    public float bpm;                  // Nhịp độ bài hát
    public string audioClipName;       // Tên file âm thanh trong Resources/Audio/
    public NoteJsonData[] notes;       // Danh sách các nốt nhạc
}
[Serializable]
public class SongListWrapper
{
    public List<SongDataModel> songs;
}
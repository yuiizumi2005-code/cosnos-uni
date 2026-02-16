using System;

[System.Serializable]
public class NovelSaveData
{
    public int storyIndex;
    public string saveTime;
    public string saveDate;
    public string playerName;  // ここを追加
    public int currentLine;    // 例：ゲームの進行状況
}

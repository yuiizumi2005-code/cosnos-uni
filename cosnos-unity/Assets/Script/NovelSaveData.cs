using System;
using System.Collections.Generic;
[System.Serializable]

public class NovelSaveData
{
    public int storyIndex;
    public string saveTime;
    public string dialogueText;   // ← ★ これを追加
    public string currentBackground;
    public string leftCharacter;
    public string rightCharacter;
    public string currentBGM;
    public string backgroundName;
    public List<CharacterSaveData> characterStates;
    
}
public class CharacterSaveData
{
    public string position;
    public string spriteName;
}

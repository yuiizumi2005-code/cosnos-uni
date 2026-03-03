using UnityEngine;
using System.IO;
using System.Collections.Generic;


public class NovelSaveManager : MonoBehaviour
{
    public CharacterManager characterManager;
    public string backgroundName;
    public static NovelSaveManager instance;
    public string bgmName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveGame(int slotNumber, int index, string dialogue, string bgName)
    {
        string savePath = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        NovelSaveData data = new NovelSaveData();
        data.storyIndex = index;
        data.saveTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        data.dialogueText = dialogue;
        data.backgroundName = bgName;

        // ✅ BGMはGameManagerから取得
        data.bgmName = NovelGameManager.instance.currentBGMName;

        // キャラ状態保存
        var state = characterManager.GetCharacterState();
        data.characterStates = new List<CharacterSaveData>();

        foreach (var pair in state)
        {
            data.characterStates.Add(new CharacterSaveData
            {
                position = pair.Key,
                spriteName = pair.Value
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("セーブ完了: " + savePath);
    }
    public NovelSaveData GetSaveData(int slotNumber)
    {
        string savePath = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<NovelSaveData>(json);
        }

    return null;
    }
    public int LoadGame(int slotNumber)
    {
        string savePath = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        if (!File.Exists(savePath))
        {
            Debug.LogWarning($"セーブファイルが存在しません: {savePath}");
            return 0;
        }

        // JSON読み込み
        string json = File.ReadAllText(savePath);

        // データを宣言
        NovelSaveData data = JsonUtility.FromJson<NovelSaveData>(json);

        if (data == null)
        {
            Debug.LogWarning($"セーブデータが破損しています: {savePath}");
            return 0;
        }

        // 背景復元
        if (!string.IsNullOrEmpty(data.backgroundName))
        {
            if (NovelGameManager.instance != null)
                NovelGameManager.instance.ChangeBackground(data.backgroundName);
            else
                Debug.LogError("NovelGameManager.instance が null です！");
        }
        if (!string.IsNullOrEmpty(data.bgmName))
        {
            BGMManager.instance.PlayBGM(data.bgmName, 1f);
        }
        // キャラ復元
        if (characterManager != null && data.characterStates != null)
        {
            Dictionary<string, string> restoreDict = new Dictionary<string, string>();

            foreach (var item in data.characterStates)
            {
                if (item != null && !string.IsNullOrEmpty(item.position))
                    restoreDict[item.position] = item.spriteName ?? "";
            }

            characterManager.RestoreCharacterState(restoreDict);
        }
        else if (characterManager == null)
        {
            Debug.LogError("characterManager がアサインされていません！");
        }

        return data.storyIndex;
    }
}
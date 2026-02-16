using UnityEngine;
using System.IO;



public class NovelSaveManager : MonoBehaviour
{
    public void SaveGame(int slotNumber, int index)
    {
        string savePath = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        NovelSaveData data = new NovelSaveData();
        data.storyIndex = index;

        // ★ 現在日時を保存
        data.saveTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");

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

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            NovelSaveData data = JsonUtility.FromJson<NovelSaveData>(json);
            return data.storyIndex;
        }

        return 0;
    }
}
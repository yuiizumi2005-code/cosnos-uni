using TMPro;
using UnityEngine;
using System.IO;
public class SaveSlotButton : MonoBehaviour
{
    public int slotNumber;
    public NovelSaveManager saveManager;
    public NovelGameManager gameManager;
    public TextMeshProUGUI infoText;   // ← スロット表示用

    void Start()
    {
        Debug.Log("Start呼ばれた");
        UpdateSlotInfo();
    }

    public void SaveThisSlot()
    {
        Debug.Log("SaveThisSlot呼ばれた");
        saveManager.SaveGame(slotNumber, gameManager.GetCurrentLine());
        UpdateSlotInfo();
    }

    void UpdateSlotInfo()
    {
        string path = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        Debug.Log("確認パス: " + path);

        if (File.Exists(path))
        {
            Debug.Log("ファイル発見");

            string json = File.ReadAllText(path);
            NovelSaveData data = JsonUtility.FromJson<NovelSaveData>(json);

            infoText.text =
                data.saveTime + "\n" +
                "第" + data.storyIndex + "行";
        }
        else
        {
            Debug.Log("ファイルなし");
            infoText.text = "空きスロット";
        }
    }
}
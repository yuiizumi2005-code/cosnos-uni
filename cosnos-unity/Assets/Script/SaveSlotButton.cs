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
        saveManager.SaveGame(slotNumber, gameManager.GetCurrentLine());
        UpdateSlotInfo();
    }

    void UpdateSlotInfo()
    {
        string path = Application.persistentDataPath + "/save" + slotNumber + ".json";

        Debug.Log(path);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            NovelSaveData data = JsonUtility.FromJson<NovelSaveData>(json);

            Debug.Log("データ読み込み成功");

            infoText.text = data.playerName;
        }
        else
        {
            Debug.Log("ファイルなし");
            infoText.text = "空きスロット";
        }
    }
}
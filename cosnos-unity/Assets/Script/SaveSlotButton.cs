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
        string currentDialogue = gameManager.dialogueText.text;

        saveManager.SaveGame(
            slotNumber,
            gameManager.GetCurrentLine(),
            currentDialogue
        );

        UpdateSlotInfo();
    }

    void UpdateSlotInfo()
    {
        string path = Application.persistentDataPath + "/save_slot_" + slotNumber + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            NovelSaveData data = JsonUtility.FromJson<NovelSaveData>(json);

            infoText.text = data.saveTime + "\n" + data.dialogueText;
        }
        else
        {
            infoText.text = "空きスロット";
        }
    }
}
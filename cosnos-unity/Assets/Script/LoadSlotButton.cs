using UnityEngine;
using TMPro;

public class LoadSlotButton : MonoBehaviour
{
    public int slotNumber;
    public NovelSaveManager saveManager;
    public NovelGameManager gameManager;
    public TextMeshProUGUI infoText;

    void Start()
    {
        UpdateSlotInfo();
    }

    public void LoadThisSlot()
    {
        NovelSaveData data = saveManager.GetSaveData(slotNumber);

        if (data != null)
        {
            NovelGameManager.instance.LoadGame(slotNumber);

            FindObjectOfType<MenuManager>().CloseMenu();
        }
    }

    void UpdateSlotInfo()
    {
        if (infoText == null)
        {
            Debug.LogWarning("infoText がアサインされていません");
            return;
        }

        NovelSaveData data = saveManager.GetSaveData(slotNumber);

        if (data != null)
        {
            infoText.text = data.saveTime + "\n" + data.dialogueText;
        }
        else
        {
            infoText.text = "空きスロット";
        }
    }
}
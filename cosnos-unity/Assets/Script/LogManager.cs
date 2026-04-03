using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public TextMeshProUGUI logText;

    public void UpdateLog()
    {
        var logs = NovelGameManager.instance.dialogueLog;

        logText.text = "";

        foreach (string line in logs)
        {
            logText.text += line + "\n";
        }
    }
}
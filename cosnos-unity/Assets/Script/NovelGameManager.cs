using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;


public class NovelGameManager : MonoBehaviour
{
    public TMP_Text nameText;         // 名前表示用Text
    public TMP_Text dialogueText;     // セリフ表示用Text
    public Image bgImage;         // 背景用Image（任意）
    public Image[] characterSlots; // 左右のキャラ表示場所（左→右）

    private List<string[]> scenarioLines = new List<string[]>();
    private int currentLine = 0;

    void Start()
    {
        foreach (Image slot in characterSlots)
    {
        slot.sprite = null;
        slot.color = Color.clear;
    }

    LoadScenario("scenario");
    DisplayLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void LoadScenario(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        StringReader reader = new StringReader(csvFile.text);

        // 1行ずつ読み込む
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            scenarioLines.Add(line.Split(',')); // カンマ区切りで分割
        }
    }

    void DisplayLine()
    {
        if (currentLine >= scenarioLines.Count) return;

        string[] parts = scenarioLines[currentLine];
        string name = parts[0];
        string text = parts[1];
        string command = parts.Length > 2 ? parts[2] : "";

        nameText.text = name;
        dialogueText.text = text;

        // キャラ操作コマンドの処理
        if (command.StartsWith("show"))
        {
            string[] c = command.Split(':');
            string target = c[0].Replace("show ", "").Trim(); // character-left-1 など
            string spriteName = c[1];
            ShowCharacter(target, spriteName);
        }
        else if (command.StartsWith("hide"))
        {
            string target = command.Replace("hide ", "").Trim();
            HideCharacter(target);
        }
    }

    void NextLine()
    {
        currentLine++;
        DisplayLine();
    }

    void ShowCharacter(string slot, string spriteName)
{
    Sprite newSprite = Resources.Load<Sprite>($"Characters/{spriteName}");
    Debug.Log($"Loading sprite: Characters/{spriteName} => {(newSprite == null ? "失敗" : "成功")}");

    if (newSprite == null)
    {
        Debug.LogError($"スプライトが見つかりません: Characters/{spriteName}");
        return;
    }

    Image target = null;
    switch (slot)
    {
        case "character-left-1": target = characterSlots[0]; break;
        case "character-left-2": target = characterSlots[1]; break;
        case "character-right-1": target = characterSlots[2]; break;
        case "character-right-2": target = characterSlots[3]; break;
    }

    if (target != null)
    {
        target.sprite = newSprite;
        target.color = Color.white;
    }
}



    void HideCharacter(string target)
    {
        foreach (Image slot in characterSlots)
        {
            if (slot.name == target)
            {
                slot.sprite = null;
                slot.color = Color.clear;
            }
        }
    }
}

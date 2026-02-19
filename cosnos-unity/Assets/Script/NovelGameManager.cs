using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class NovelGameManager : MonoBehaviour
{
    public TMP_Text nameText;         // 名前表示
    public TMP_Text dialogueText;     // セリフ表示
    public Image bgImage;             // 背景
    public Image[] characterSlots;    // 立ち絵スロット（配列で左→右）
    public Image fadePanel;
    public float fadeSpeed = 2f;


    private List<string[]> scenarioLines = new List<string[]>();
    public NovelSaveManager saveManager;
    public int currentLine = 0;
    public bool isMenuOpen = false;
    public bool isFading = false;


    void Start()
    {
        Debug.Log("NovelGameManager Start 呼ばれた");
        // 立ち絵を初期化
        foreach (Image slot in characterSlots)
        {
            slot.sprite = null;
            slot.color = Color.clear;
        }

        LoadScenario("scenario"); // Resources/scenario.csv
        DisplayLine();
    }


    void Update()
    {
        // フェード中は進まない
        if (isFading) return;

        // メニューがアクティブなら進まない
        if (GameObject.Find("MenuPanel") != null &&
            GameObject.Find("MenuPanel").activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void LoadScenario(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile == null)
        {
            Debug.LogError("CSVが見つかりません: " + fileName);
            return;
        }

        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            scenarioLines.Add(line.Split(','));
        }
    }

    void DisplayLine()
    {
        if (currentLine >= scenarioLines.Count) return;

        string[] parts = scenarioLines[currentLine];

        string name = parts.Length > 0 ? parts[0] : "";
        string text = parts.Length > 1 ? parts[1] : "";
        string command = parts.Length > 2 ? parts[2] : "";

        // 名前とセリフ表示
        nameText.text = name;
        dialogueText.text = text;

        // ===== コマンド処理 =====

        // 背景変更
        if (command.StartsWith("bg "))
        {
            string bgName = command.Replace("bg ", "").Trim();
            ChangeBackground(bgName);
        }
        // 立ち絵表示
        else if (command.StartsWith("show "))
        {
            string[] c = command.Split(':');
            if (c.Length >= 2)
            {
                string slot = c[0].Replace("show ", "").Trim();
                string spriteName = c[1].Trim();
                ShowCharacter(slot, spriteName);
            }
        }
        // 立ち絵非表示
        else if (command.StartsWith("hide "))
        {
            string slot = command.Replace("hide ", "").Trim();
            HideCharacter(slot);
        }
        // フェードアウト（終わるまで待つ）
        else if (command == "fadeout")
        {
            StartCoroutine(FadeOutAndNext());
            return;
        }
        // フェードイン（終わるまで待つ）
        else if (command == "fadein")
        {
            StartCoroutine(FadeInAndNext());
            return;
        }
        // BGM変更
        else if (command.StartsWith("bgm "))
        {
            string bgmName = command.Replace("bgm ", "").Trim();
            BGMManager.instance.PlayBGM(bgmName);
        }
    }

    void NextLine()
    {
        currentLine++;
        DisplayLine();
    }

    // ===== 背景変更 =====
    void ChangeBackground(string spriteName)
    {
        Sprite bg = Resources.Load<Sprite>("Backgrounds/" + spriteName);

        Debug.Log("読み込みに行っているパス: Backgrounds/" + spriteName);
        Debug.Log("結果: " + (bg == null ? "null（失敗）" : "成功"));

        if (bg == null)
        {
            Debug.LogError("背景が見つかりません: " + spriteName);
            return;
        }

        bgImage.sprite = bg;
        bgImage.color = Color.white;
    }


    // ===== 立ち絵表示 =====
    void ShowCharacter(string slotName, string spriteName)
    {
        Sprite newSprite = Resources.Load<Sprite>("Characters/" + spriteName);

        if (newSprite == null)
        {
            Debug.LogError("スプライトが見つかりません: " + spriteName);
            return;
        }

        Image target = null;

        switch (slotName)
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

    // ===== 立ち絵非表示 =====
    void HideCharacter(string slotName)
    {
        Image target = null;

        switch (slotName)
        {
            case "character-left-1": target = characterSlots[0]; break;
            case "character-left-2": target = characterSlots[1]; break;
            case "character-right-1": target = characterSlots[2]; break;
            case "character-right-2": target = characterSlots[3]; break;
        }

        if (target != null)
        {
            target.sprite = null;
            target.color = Color.clear;
        }
    }

    IEnumerator FadeOutAndNext()
    {
        isFading = true;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        isFading = false;

        currentLine++;
        DisplayLine();
    }

    IEnumerator FadeInAndNext()
    {
        isFading = true;

        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        isFading = false;

        currentLine++;
        DisplayLine();
    }
    public void SaveGame(int slotNumber)
    {
        string currentDialogue = dialogueText.text;   // 今表示中のセリフ
        saveManager.SaveGame(slotNumber, currentLine, currentDialogue);
    }
    public void LoadGame(int slotNumber)
    {
        currentLine = saveManager.LoadGame(slotNumber);
        DisplayLine();
    }
    public int GetCurrentLine()
    {
        return currentLine;
    }
}




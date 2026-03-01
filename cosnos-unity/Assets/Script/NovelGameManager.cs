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
     public static NovelGameManager instance;
    public bool isPlayingMovie = false;

    public string currentBackgroundName;
    public string currentLeftCharacter;
    public string currentRightCharacter;
    public string currentBGMName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えでも残す場合
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        BGMManager.instance.PlayBGM("school_theme", 0.5f);

        Debug.Log("NovelGameManager Start 呼ばれた");

        // 立ち絵初期化
        foreach (Image slot in characterSlots)
        {
            slot.sprite = null;
            slot.color = Color.clear;
        }

        // シナリオ読み込み
        LoadScenario("scenario"); // Resources/scenario.csv

        // **ここでは自動ロードはせず、ボタン経由でロードする**
        DisplayLine();
    }


    void Update()
    {
        // フェード中は進まない
        if (isFading) return;
        if (isPlayingMovie) return;

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
        if (isPlayingMovie) return;

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
    public int GetCurrentLine()
    {
        return currentLine;
    }
    void DisplayLine()
    {
         if (currentLine >= scenarioLines.Count)
        {
            ChapterEndManager endManager = FindObjectOfType<ChapterEndManager>();
            if (endManager != null)
            {
                endManager.GoToMessageScene();
            }
            return;
        }

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
            string[] bgmParts = command.Replace("bgm ", "").Split(' ');
            string bgmName = bgmParts[0];
            float volume = 1f;

            if (bgmParts.Length >= 2)
            {
                float.TryParse(bgmParts[1], out volume);
            }

            BGMManager.instance.PlayBGM(bgmName, volume);
        }
        else if (command.StartsWith("movie "))
        {
            string movieName = command.Replace("movie ", "").Trim();
            VideoManager.instance.PlayVideo(movieName);
        }


    }
    public void NextLine()
    {
        currentLine++;
        DisplayLine();
    }

    // ===== 背景変更 =====
    public void ChangeBackground(string spriteName)
    {
        currentBackgroundName = spriteName;   // ★ここで必ず更新

        Sprite bg = Resources.Load<Sprite>("Backgrounds/" + spriteName);

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
        string currentDialogue = dialogueText.text;

        saveManager.SaveGame(
            slotNumber,
            currentLine,
            currentDialogue,
            currentBackgroundName   // ★追加
        );
    }
    public void LoadGame(int slotNumber)
    {
        if (NovelSaveManager.instance == null)
        {
            Debug.LogError("SaveManagerが存在しません");
            return;
        }

        if (scenarioLines == null || scenarioLines.Count == 0)
        {
            Debug.LogWarning("シナリオが読み込まれていません");
            return;
        }

        currentLine = NovelSaveManager.instance.LoadGame(slotNumber);

        if (currentLine >= scenarioLines.Count)
            currentLine = scenarioLines.Count - 1;
        if (currentLine < 0)
            currentLine = 0;

        DisplayLine();
    }
    public string GetCurrentBackgroundName()
    {
        return currentBackgroundName;
    }

}




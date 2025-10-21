using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class DialogueLine
{
    public string name;
    public string text;
}

public static class ScenarioLoader
{
    public static List<DialogueLine> LoadScenario(string fileName)
    {
        List<DialogueLine> lines = new List<DialogueLine>();

        // ファイルパス（StreamingAssets内を指定）
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // ファイルが存在するかチェック
        if (!File.Exists(filePath))
        {
            Debug.LogError("シナリオファイルが見つかりません: " + filePath);
            return lines;
        }

        // 行ごとに読み込む
        string[] allLines = File.ReadAllLines(filePath);

        // 1行目はヘッダーなのでスキップ
        for (int i = 1; i < allLines.Length; i++)
        {
            string[] cols = allLines[i].Split(',');

            if (cols.Length >= 2)
            {
                DialogueLine line = new DialogueLine();
                line.name = cols[0];
                line.text = cols[1];
                lines.Add(line);
            }
        }

        return lines;
    }
}

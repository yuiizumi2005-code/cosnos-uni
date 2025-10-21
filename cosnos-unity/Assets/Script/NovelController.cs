void ShowLine()
{
    if (currentLine < lines.Count)
    {
        string name = lines[currentLine].name;
        string text = lines[currentLine].text;

        // 名前が空欄ならナレーションとして扱う
        if (string.IsNullOrEmpty(name))
        {
            nameText.text = "";  // 名前欄は消す
        }
        else
        {
            nameText.text = name;
        }

        dialogueText.text = text;
    }
    else
    {
        nameText.text = "";
        dialogueText.text = "（終わり）";
    }
}

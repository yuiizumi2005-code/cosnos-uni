using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterSlot
    {
        public string position; // 例: "character-right-1"
        public Image image;     // 対応するImage
    }

    public List<CharacterSlot> characterSlots; // Unityで登録
    private Dictionary<string, Image> slotDict;
    private Dictionary<string, string> currentCharacterState;
    void Awake()
    {
        slotDict = new Dictionary<string, Image>();
        currentCharacterState = new Dictionary<string, string>();

        foreach (var slot in characterSlots)
        {
            slotDict[slot.position] = slot.image;
            currentCharacterState[slot.position] = null; // 初期化
        }
    }

    public void ShowCharacter(string position, Sprite sprite)
    {
        if (slotDict.ContainsKey(position))
        {
            slotDict[position].sprite = sprite;
            slotDict[position].color = new Color(1, 1, 1, 1);

            // 🔥 追加
            currentCharacterState[position] = sprite.name;
        }
    }

    public void HideCharacter(string position)
    {
        if (slotDict.ContainsKey(position))
        {
            slotDict[position].sprite = null;
            slotDict[position].color = new Color(1, 1, 1, 0);

            // 🔥 追加
            currentCharacterState[position] = null;
        }
    }
    public Dictionary<string, string> GetCharacterState()
    {
        return new Dictionary<string, string>(currentCharacterState);
    }
    public void RestoreCharacterState(Dictionary<string, string> savedState)
    {
        foreach (var pair in savedState)
        {
            if (string.IsNullOrEmpty(pair.Value))
            {
                HideCharacter(pair.Key);
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>("Characters/" + pair.Value);
                ShowCharacter(pair.Key, sprite);
            }
        }
    }
}

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

    void Awake()
    {
        slotDict = new Dictionary<string, Image>();
        foreach (var slot in characterSlots)
        {
            slotDict[slot.position] = slot.image;
        }
    }

    public void ShowCharacter(string position, Sprite sprite)
    {
        if (slotDict.ContainsKey(position))
        {
            slotDict[position].sprite = sprite;
            slotDict[position].color = new Color(1, 1, 1, 1); // 表示
        }
        else
        {
            Debug.LogWarning($"位置 {position} が見つかりません。");
        }
    }

    public void HideCharacter(string position)
    {
        if (slotDict.ContainsKey(position))
        {
            slotDict[position].sprite = null;
            slotDict[position].color = new Color(1, 1, 1, 0); // 非表示
        }
    }
}

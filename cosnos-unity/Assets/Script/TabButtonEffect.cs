using UnityEngine;

public class TabButtonEffect : MonoBehaviour
{
    private Vector2 originalPos;
    public float moveAmount = 250f; // 右に動く量

    void Start()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public void SelectTab()
    {
        GetComponent<RectTransform>().anchoredPosition =
            originalPos + new Vector2(moveAmount, 0);
    }

    public void ResetTab()
    {
        GetComponent<RectTransform>().anchoredPosition = originalPos;
    }
}

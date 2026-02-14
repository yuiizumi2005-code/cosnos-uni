using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;   // メニューのPanel

    private bool isPaused = false;

    // メニューボタン用
    public void OpenMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;   // ゲーム停止
        isPaused = true;
    }

    // Backボタン用
    public void CloseMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;   // ゲーム再開
        isPaused = false;
    }

    // もしトグル（開閉切り替え）で使いたい場合
    public void ToggleMenu()
    {
        if (isPaused)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}

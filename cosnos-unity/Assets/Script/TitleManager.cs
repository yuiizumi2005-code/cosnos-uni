using UnityEngine;
using UnityEngine.SceneManagement; // ←これ必須

public class TitleManager : MonoBehaviour
{
    // ボタンに設定するメソッド
    public void StartGame()
    {
        // シーン名で指定する場合
        SceneManager.LoadScene("Chapter0");

        // シーン番号で指定する場合（Build Settingsで確認した番号）
        // SceneManager.LoadScene(1);
    }
}

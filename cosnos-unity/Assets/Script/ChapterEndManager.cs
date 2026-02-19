using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterEndManager : MonoBehaviour
{
    public void GoToMessageScene()
    {
        SceneManager.LoadScene(2);
    }
}

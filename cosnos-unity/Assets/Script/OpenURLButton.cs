using UnityEngine;

public class OpenURLButton : MonoBehaviour
{
    public string url = "https://x.com/chiyu449377";

    public void Open()
    {
        Application.OpenURL(url);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainTitle");
        }
    }

}

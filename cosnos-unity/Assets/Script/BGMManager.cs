using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public AudioSource audioSource;
    public float fadeTime = 1.5f;

    void Awake()
    {
        instance = this;   // ★ これを追加！！

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D
    }

    public void PlayBGM(string bgmName)
    {
        Debug.Log("BGM再生指示: " + bgmName);

        string path = "BGM/" + bgmName;
        Debug.Log("読み込みパス: " + path);

        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            Debug.LogError("BGMが見つかりません: " + bgmName);
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();

        Debug.Log("再生しました");
    }

    IEnumerator FadeBGM(AudioClip newClip)
    {
        // フェードアウト
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // フェードイン
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = 1;
    }
    
}
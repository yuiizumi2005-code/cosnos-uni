using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public AudioSource audioSource;
    public float fadeTime = 1.5f;
    public string currentBGMName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSourceが未設定なら自動取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private Coroutine fadeCoroutine;

    public void PlayBGM(string bgmName, float volume)
    {   currentBGMName = bgmName;
        AudioClip clip = Resources.Load<AudioClip>("BGM/" + bgmName);

        if (clip == null)
        {
            Debug.LogError("BGMが見つかりません: " + bgmName);
            return;
        }

        // ★ 前のフェードを止める
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeBGM(clip, volume));
    }




    IEnumerator FadeBGM(AudioClip newClip, float targetVolume)
    {
        // フェードアウト
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.volume = 0f;
        audioSource.Play();

        // フェードイン
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    
}
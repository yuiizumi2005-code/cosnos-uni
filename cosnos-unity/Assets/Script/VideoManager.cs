using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    [Header("Video Player Settings")]
    public VideoPlayer videoPlayer;       // VideoPlayer コンポーネント
    public GameObject videoObject;        // VideoPlayer をアタッチした GameObject
    public AudioSource audioSource;       // 音声再生用 AudioSource

    private void Awake()
    {
        instance = this;

        videoObject.SetActive(false);

        // VideoPlayer 設定
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane; // 画面全体表示
        videoPlayer.targetCamera = Camera.main;                  // Main Camera に描画
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }


    /// <summary>
    /// StreamingAssets 内の動画を再生
    /// </summary>
    /// <param name="fileName">動画ファイル名（拡張子込み）</param>
    public void PlayVideo(string fileName)
    {
        StartCoroutine(PlayCoroutine(fileName));
    }

    private IEnumerator PlayCoroutine(string fileName)
    {
        if (NovelGameManager.instance != null)
            NovelGameManager.instance.isPlayingMovie = true;

        string path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log("再生する動画のパス: " + path);
        Debug.Log("Video Path: " + path);

        videoObject.SetActive(true);
        videoPlayer.url = path;

        // 動画を準備
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);

        // 再生開始
        videoPlayer.Play();
        audioSource.Play();

        // 再生終了まで待機
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        // 再生終了後、非表示
        videoObject.SetActive(false);

        if (NovelGameManager.instance != null)
            NovelGameManager.instance.isPlayingMovie = false;
    }
}

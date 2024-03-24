using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] public AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private Dictionary<string, int> bgmIndexByScene;

    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;

        bgmIndexByScene = new Dictionary<string, int>();
        bgmIndexByScene.Add("MainMenu", 8);
        bgmIndexByScene.Add("Level0", 0);
        bgmIndexByScene.Add("LevelArena", 6);
    }

    private void Start()
    {
        PlayBGMByScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        // if (sfx[_sfxIndex].isPlaying) return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance) return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public IEnumerator IncreaseVolumeOverTime(int _sfxIndex, float currentVolume, float fadeTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float startVolume = currentVolume;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeTime;
            float targetVolume = Mathf.Lerp(startVolume, 1f, normalizedTime);
            sfx[_sfxIndex].volume = targetVolume;
            yield return null;
        }

        sfx[_sfxIndex].volume = 1f;
    }

    public IEnumerator DecreaseVolumeOverTime(int _sfxIndex, float currentVolume, float fadeTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float startVolume = currentVolume;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeTime;
            float targetVolume = Mathf.Lerp(startVolume, 0f, normalizedTime);
            sfx[_sfxIndex].volume = targetVolume;
            yield return null;
        }

        sfx[_sfxIndex].Stop();
        sfx[_sfxIndex].volume = startVolume;
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        // StopAllBGM();
        bgm[bgmIndex].Play();

        StartCoroutine(FadeInAndPlay(SceneManager.GetActiveScene().name));
    }

    private IEnumerator FadeInAndPlay(string sceneName)
    {
        float fadeDuration = 1.5f; // 渐变时长

        if (bgmIndexByScene.ContainsKey(sceneName))
        {
            int bgmIndex = bgmIndexByScene[sceneName];
        }

        AudioSource audioSource = bgm[bgmIndex]; // 获取音频源
        audioSource.volume = 0f; // 将音量初始值设为 0
        audioSource.Play(); // 开始播放音频源

        float startTime = Time.time; // 记录渐变开始时间
        float elapsedTime = 0f; // 记录已经过去的时间

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration; // 计算渐变进度
            float volume = Mathf.Lerp(0f, 1f, t); // 使用Lerp函数进行渐变

            audioSource.volume = volume; // 设置音量

            elapsedTime = Time.time - startTime; // 更新已经过去的时间
            yield return null; // 等待下一帧
        }

        audioSource.volume = 1f; // 将音量设为最大值
    }

    public void StopAllBGM()
    {
        StartCoroutine(FadeOutAndStop(SceneManager.GetActiveScene().name));
    }

    private IEnumerator FadeOutAndStop(string sceneName)
    {
        float fadeDuration = 1.5f; // 渐变时长

        if (bgmIndexByScene.ContainsKey(sceneName))
        {
            int bgmIndex = bgmIndexByScene[sceneName];
        }
        
        AudioSource audioSource = bgm[bgmIndex]; // 获取音频源

        float startVolume = audioSource.volume; // 初始音量
        float startTime = Time.time; // 记录渐变开始时间
        float elapsedTime = 0f; // 记录已经过去的时间

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration; // 计算渐变进度
            float volume = Mathf.Lerp(startVolume, 0f, t); // 使用Lerp函数进行渐变

            audioSource.volume = volume; // 设置音量

            elapsedTime = Time.time - startTime; // 更新已经过去的时间
            yield return null; // 等待下一帧
        }

        audioSource.Stop(); // 停止音频源
        audioSource.volume = startVolume; // 恢复初始音量
    }

    private void PlayBGMByScene(string sceneName)
    {
        if (bgmIndexByScene.ContainsKey(sceneName))
        {
            int bgmIndex = bgmIndexByScene[sceneName];
            PlayBGM(bgmIndex);
        }
        else
        {
            // 如果没有为当前场景指定 BGM 索引，则停止所有 BGM
            StopAllBGM();
        }
    }
}

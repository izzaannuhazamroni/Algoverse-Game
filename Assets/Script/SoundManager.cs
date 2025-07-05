using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("Music Settings")]
    public Slider musicSlider;
    public AudioSource musicSource;
    public AudioClip backgroundMusic; // Musik untuk Scene1
    public AudioClip musicScene3;

    [Header("SFX Settings")]
    public Slider sfxSlider;
    public AudioSource sfxSource;
    public AudioClip clickSound;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // expose param: MusicVolume, SFXVolume

    void Awake()
    {
        int smCount = FindObjectsOfType<SoundManager>().Length;
        if (smCount > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load dan set Music Volume
        float savedMusic = PlayerPrefs.GetFloat("musicVol", 1f);
        musicSlider.value = savedMusic;
        SetMusicVolume(savedMusic);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        // Load dan set SFX Volume
        float savedSFX = PlayerPrefs.GetFloat("sfxVol", 1f);
        sfxSlider.value = savedSFX;
        SetSFXVolume(savedSFX);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Mainkan BGM jika ada
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(default, SceneManager.GetActiveScene());
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("musicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("sfxVol", value);
    }

    public void PlayClickSound()
    {
        if (clickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }

    public void MuteAllSound()
    {
        musicSlider.value = 0f;
        sfxSlider.value = 0f;
        SetMusicVolume(0f);
        SetSFXVolume(0f);
    }

    public void ResetAllSound()
    {
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
        SetMusicVolume(1f);
        SetSFXVolume(1f);
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        switch (newScene.name)
        {
            case "MainMenu":
                PlayMusic(backgroundMusic);
                break;

            case "CutScene1":
                StopMusic();
                break;

            case "Maps":
                PlayMusic(musicScene3);
                break;

            case "Lab1":
                PlayMusic(musicScene3);
                break;

            default:
                StopMusic();
                break;
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    
}

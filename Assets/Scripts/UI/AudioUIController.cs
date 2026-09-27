using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioManager audioManager;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button sfxTestButton;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambientSlider;

    [Header("Labels")]
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;
    [SerializeField] private TMP_Text ambientValueText;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string AmbientVolumeKey = "AmbientVolume";
    public int currentSFXIndex = 0;

    private void Start()
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        LoadSettings();
        ConfigureButtons();
        ConfigureSliders();
    }

    private void ConfigureButtons()
    {
        playButton?.onClick.AddListener(audioManager.PlayMusic);
        pauseButton?.onClick.AddListener(audioManager.PauseMusic);
        stopButton?.onClick.AddListener(audioManager.StopMusic);
        nextButton?.onClick.AddListener(audioManager.NextTrack);
        sfxTestButton?.onClick.AddListener(PlayNextSFX);
    }

    private void ConfigureSliders()
    {
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = audioManager.MusicVolume;
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            sfxSlider.value = audioManager.SfxVolume;
        }

        if (ambientSlider != null)
        {
            ambientSlider.minValue = 0f;
            ambientSlider.maxValue = 1f;
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
            ambientSlider.value = audioManager.AmbientVolume;
        }

        UpdateLabels();
    }

    private void SetMusicVolume(float value)
    {
        audioManager.MusicVolume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        UpdateLabels();
        PlayerPrefs.Save();
    }

    private void PlayNextSFX()
    {
        if (audioManager == null)
            return;

        audioManager.PlaySFX(currentSFXIndex);

        currentSFXIndex++;

        if (currentSFXIndex >= audioManager.SFXCount)
            currentSFXIndex = 0;
    }

    private void SetSfxVolume(float value)
    {
        audioManager.SfxVolume = value;
        PlayerPrefs.SetFloat(SfxVolumeKey, value);
        UpdateLabels();
        PlayerPrefs.Save();
    }

    private void SetAmbientVolume(float value)
    {
        audioManager.AmbientVolume = value;
        PlayerPrefs.SetFloat(AmbientVolumeKey, value);
        UpdateLabels();
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float music = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        float ambient = PlayerPrefs.GetFloat(AmbientVolumeKey, 1f);

        audioManager.MusicVolume = music;
        audioManager.SfxVolume = sfx;
        audioManager.AmbientVolume = ambient;
    }

    private void UpdateLabels()
    {
        if (musicValueText != null)
            musicValueText.text = $"ћузыка: {audioManager.MusicVolume * 100f:0}%";

        if (sfxValueText != null)
            sfxValueText.text = $"Ёффекты: {audioManager.SfxVolume * 100f:0}%";

        if (ambientValueText != null)
            ambientValueText.text = $"јмбиент: {audioManager.AmbientVolume * 100f:0}%";
    }
}

using UnityEngine;
using UnityEngine.UI;

public class AudioVisualizer : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Visualizer Bars")]
    [SerializeField] private Image[] bars = new Image[8];

    [Header("FFT Settings")]
    [SerializeField] private int spectrumSize = 1024;
    [SerializeField] private FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    [Header("Frequency Range")]
    [SerializeField] private float minFrequency = 60f;
    [SerializeField] private float maxFrequency = 16000f;

    [Header("Visual Settings")]
    [SerializeField] private float minDb = -70f;
    [SerializeField] private float maxDb = -10f;
    [SerializeField] private float sensitivity = 1.5f;

    [Header("Animation")]
    [SerializeField] private float attackSpeed = 20f;
    [SerializeField] private float releaseSpeed = 8f;

    private float[] spectrum;
    private float[] currentLevels;

    private const int BarCount = 8;

    private void Awake()
    {
        spectrumSize = Mathf.ClosestPowerOfTwo(spectrumSize);
        spectrumSize = Mathf.Clamp(spectrumSize, 64, 8192);

        spectrum = new float[spectrumSize];
        currentLevels = new float[BarCount];
    }

    private void Update()
    {
        if (audioSource == null)
            return;

        if (bars == null || bars.Length < BarCount)
            return;

        audioSource.GetSpectrumData(
            spectrum,
            0,
            fftWindow
        );

        for (int i = 0; i < BarCount; i++)
        {
            float startFrequency = GetBandStartFrequency(i);
            float endFrequency = GetBandEndFrequency(i);

            float level = GetFrequencyBandLevel(
                startFrequency,
                endFrequency
            );

            level *= sensitivity;

            level = Mathf.Clamp01(level);

            float speed = level > currentLevels[i]
                ? attackSpeed
                : releaseSpeed;

            currentLevels[i] = Mathf.Lerp(
                currentLevels[i],
                level,
                Time.deltaTime * speed
            );

            bars[i].fillAmount = currentLevels[i];
        }
    }

    private float GetBandStartFrequency(int band)
    {
        float logMin = Mathf.Log10(minFrequency);
        float logMax = Mathf.Log10(maxFrequency);

        float t = band / (float)BarCount;

        return Mathf.Pow(
            10f,
            Mathf.Lerp(logMin, logMax, t)
        );
    }

    private float GetBandEndFrequency(int band)
    {
        float logMin = Mathf.Log10(minFrequency);
        float logMax = Mathf.Log10(maxFrequency);

        float t = (band + 1) / (float)BarCount;

        return Mathf.Pow(
            10f,
            Mathf.Lerp(logMin, logMax, t)
        );
    }

    private float GetFrequencyBandLevel(
        float startFrequency,
        float endFrequency)
    {
        float sampleRate = AudioSettings.outputSampleRate;

        float frequencyPerBin =
            sampleRate / spectrumSize;

        int startIndex =
            Mathf.FloorToInt(startFrequency / frequencyPerBin);

        int endIndex =
            Mathf.CeilToInt(endFrequency / frequencyPerBin);

        startIndex = Mathf.Clamp(
            startIndex,
            0,
            spectrum.Length - 1
        );

        endIndex = Mathf.Clamp(
            endIndex,
            startIndex + 1,
            spectrum.Length
        );

        float sum = 0f;
        int count = 0;

        for (int i = startIndex; i < endIndex; i++)
        {
            float value = spectrum[i];

            sum += value * value;
            count++;
        }

        if (count == 0)
            return 0f;

        float rms = Mathf.Sqrt(sum / count);

        if (rms <= 0.000001f)
            return 0f;

        float db = 20f * Mathf.Log10(rms);

        float normalized = Mathf.InverseLerp(
            minDb,
            maxDb,
            db
        );

        return normalized;
    }
}
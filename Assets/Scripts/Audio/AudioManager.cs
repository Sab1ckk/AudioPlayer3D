using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Playlists")]
    [SerializeField] private List<AudioClip> musicPlaylist = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ambientClips = new List<AudioClip>();

    private int currentTrackIndex;
    public int SFXCount => sfxClips.Count;

    public float MusicVolume
    {
        get => musicSource != null ? musicSource.volume : 0f;
        set
        {
            if (musicSource != null)
                musicSource.volume = Mathf.Clamp01(value);
        }
    }

    public float SfxVolume
    {
        get => sfxSource != null ? sfxSource.volume : 0f;
        set
        {
            if (sfxSource != null)
                sfxSource.volume = Mathf.Clamp01(value);
        }
    }

    public float AmbientVolume
    {
        get => ambientSource != null ? ambientSource.volume : 0f;
        set
        {
            if (ambientSource != null)
                ambientSource.volume = Mathf.Clamp01(value);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayMusic()
    {
        if (musicSource == null || musicPlaylist.Count == 0)
            return;

        if (musicSource.clip == null)
            musicSource.clip = musicPlaylist[currentTrackIndex];

        musicSource.Play();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void NextTrack()
    {
        if (musicSource == null || musicPlaylist.Count == 0)
            return;

        currentTrackIndex = (currentTrackIndex + 1) % musicPlaylist.Count;

        musicSource.clip = musicPlaylist[currentTrackIndex];
        musicSource.Play();
    }

    public void PreviousTrack()
    {
        if (musicSource == null || musicPlaylist.Count == 0)
            return;

        currentTrackIndex =
            (currentTrackIndex - 1 + musicPlaylist.Count) %
            musicPlaylist.Count;

        musicSource.clip = musicPlaylist[currentTrackIndex];
        musicSource.Play();
    }

    public void PlaySFX(int index = 0)
    {
        if (sfxSource == null || sfxClips.Count == 0)
            return;

        index = Mathf.Clamp(index, 0, sfxClips.Count - 1);
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    public void PlayAmbient(int index = 0)
    {
        if (ambientSource == null || ambientClips.Count == 0)
            return;

        index = Mathf.Clamp(index, 0, ambientClips.Count - 1);
        ambientSource.clip = ambientClips[index];
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public AudioSource GetMusicSource() => musicSource;
}

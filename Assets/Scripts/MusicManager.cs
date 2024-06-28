using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : PersistentSingleton<MusicManager>
{
    private AudioSource _musicSource;
    private bool _isMuted;
    [HideInInspector]
    public float _volume {  get; set; }

    private float _baseVolume;

    public void ChangeMusic(AudioClip song, float fadeOutDuration = 0.5f)
    {
        StartCoroutine(ChangeMusicTransition(song, fadeOutDuration));
    }

    public void ChangeMusic(AudioClip song, float volumeForThisPiece, float fadeOutDuration = 0.5f)
    {
        StartCoroutine(ChangeMusicTransition(song, volumeForThisPiece, fadeOutDuration));
    }

    private void Start()
    {
        _musicSource = gameObject.GetComponent<AudioSource>();
        _volume = _musicSource.volume;
        _baseVolume = _musicSource.volume;
    }

    private void Update()
    {
        ToggleMuteMusic();
    }

    public void ChangeVolumeForCurrentTrackTo(float volume, float fadeDuration = 0.5f)
    {
        StartCoroutine(ChangeVolumeTransition(volume, fadeDuration));
    }

    private IEnumerator ChangeVolumeTransition(float volume, float fadeOutDuration)
    {
        float currentTime = 0.0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(_volume, volume, currentTime / fadeOutDuration);
            yield return null;
        }
        _volume = volume;
    }


    private IEnumerator ChangeMusicTransition(AudioClip song, float volume, float fadeOutDuration)
    {
        float currentTime = 0.0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(_volume, 0.0f, currentTime / fadeOutDuration);
            yield return null;
        }
        _volume = volume;
        _musicSource.clip = song;
        if (!_isMuted)
        {
            _musicSource.volume = volume;
            _musicSource.Play();
        }
    }

    private IEnumerator ChangeMusicTransition(AudioClip song, float fadeOutDuration)
    {
        float currentTime = 0.0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(_volume, 0.0f, currentTime / fadeOutDuration);
            yield return null;
        }
        _volume = _baseVolume;
        _musicSource.clip = song;
        if (!_isMuted)
        {
            _musicSource.volume = _volume;
            _musicSource.Play();
        }
    }

    private IEnumerator FadeToSilence(float fadeOutDuration)
    {
        float currentTime = 0.0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(_volume, 0.0f, currentTime / fadeOutDuration);
            yield return null;
        }

        _musicSource.Pause();
    }

    private void ToggleMuteMusic()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_isMuted)
            {
                _isMuted = false;
                _musicSource.UnPause();
            }
            else
            {
                _isMuted = true;
                _musicSource.Pause();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VignetteSceneManager : MonoBehaviour
{
    public bool fadeComplete = false;

    [SerializeField] private TextMeshProUGUI _topText;
    [SerializeField] private TextMeshProUGUI _midText;
    [SerializeField] private TextMeshProUGUI _botText;
    [SerializeField] private string _vignetteText;
    [SerializeField] private float _textTimer = 1f;
    [SerializeField] private Image _mainVignette;
    [SerializeField] private float _fadeRate = 0.5f;
    [SerializeField] private GameObject _vignetteBase;
    [SerializeField] private AudioClip _voiceLine;
    private AudioSource _audioSource;

    private float _targetAlpha = 1;
    private bool _showTopText = true;
    private bool _showBotText = true;
    private bool _showMidText = true;
    private List<string> _topBotDivider = new List<string>();

    public bool GetIsDoneShowingText()
    {
        if (_topText != null && _topText.GetComponent<TextScroller>().GetIsRollingText() == true)
        {
            return false;
        }
        if (_midText != null && _midText.GetComponent<TextScroller>().GetIsRollingText() == true)
        {
            return false;
        }
        if (_botText != null && _botText.GetComponent<TextScroller>().GetIsRollingText() == true)
        {
            return false;
        }

        return true;
    }

    public void ShowAllText()
    {
        _topText?.GetComponent<TextScroller>().ShowAllText();
        _midText?.GetComponent<TextScroller>().ShowAllText();
        _botText?.GetComponent<TextScroller>().ShowAllText();
    }

    public void PauseAudio()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();

        if (_mainVignette != null) StartCoroutine(FadeIn());

        if (_topText != null && _botText != null)
        {
            _topBotDivider.Add(_vignetteText.Substring(0, _vignetteText.IndexOf("/n ")));
            _topBotDivider.Add(_vignetteText.Substring(_vignetteText.IndexOf("/n ") + 3));
        }
    }

    private void Start()
    {
        PlayAudio();
    }

    private void Update()
    {
        //This is a bunch of if statements which fill and call display methods of the text elements, the if statements are to ensure the "right" text elements are used
        if (_topText != null && fadeComplete == true && _showTopText == true)
        {
            _topText.GetComponent<TextScroller>().DisplayText(_topBotDivider[0], _textTimer);
            _showTopText = false;
        }
        if (_topText != null && _botText != null && _topText.GetComponent<TextScroller>().GetIsRollingText() == false && _showBotText == true)
        {
            _botText.GetComponent<TextScroller>().DisplayText((" " + _topBotDivider[1]), _textTimer);
            _showBotText = false;
        }
        if (_botText != null && _topText == null && _botText.GetComponent<TextScroller>().GetIsRollingText() == false && _showBotText == true)
        {
            _botText.GetComponent<TextScroller>().DisplayText(_vignetteText, _textTimer);
            _showBotText = false;
        }

        if (_midText != null && _midText.GetComponent<TextScroller>().GetIsRollingText() == false && _showMidText == true)
        {
            _midText.GetComponent<TextScroller>().DisplayText(_vignetteText, _textTimer);
            _showMidText = false;
        }
    }

    private IEnumerator FadeIn()
    {
        _targetAlpha = 1.0f;
        Color curColor = _mainVignette.color;
        while (Mathf.Abs(curColor.a - _targetAlpha) > 0.0001f)
        {

            curColor.a = Mathf.Lerp(curColor.a, _targetAlpha, _fadeRate * Time.deltaTime);
            _mainVignette.color = curColor;
            yield return null;
        }
        curColor.a = _targetAlpha; _mainVignette.color = curColor;
        fadeComplete = true;
    }

    public IEnumerator FadeOut()
    {
        fadeComplete = false;
        _targetAlpha = 0f;
        Color curColor = _mainVignette.color;
        while (Mathf.Abs(curColor.a - _targetAlpha) > 0.0001f)
        {

            curColor.a = Mathf.Lerp(curColor.a, _targetAlpha, _fadeRate * Time.deltaTime);
            _mainVignette.color = curColor;
            yield return null;
        }
        curColor.a = _targetAlpha; _mainVignette.color = curColor;
        fadeComplete = true;
    }

    private void PlayAudio()
    {
        if (_voiceLine != null && _audioSource != null)
        {
            _audioSource.clip = _voiceLine;
            _audioSource.Play();
        }
    }
}

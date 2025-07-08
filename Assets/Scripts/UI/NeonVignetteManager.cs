using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeonVignetteManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _vignetteSprites;
    [SerializeField] private List<float> _vignetteTimers;
    [SerializeField] private List<string> _vignetteTexts;

    [SerializeField] private Image _vignetteImage;
    [SerializeField] private TextMeshProUGUI _topTextGUI;
    [SerializeField] private TextMeshProUGUI _middleTextGUI;
    [SerializeField] private TextMeshProUGUI _bottomTextGUI;
    [SerializeField] private TextMeshProUGUI _scrollTextGUI;

    [SerializeField] private AudioClip _vignetteAudio1;
    [SerializeField] private AudioClip _vignetteAudio1_SignatureOnly;
    [SerializeField] private AudioClip _scribbleAudio;
    [SerializeField] private AudioClip _vignetteAudio2;
    [SerializeField] private AudioClip _vignetteAudio2_OutroLaughOnly;

    private AudioSource _audioSource;
    int _curSpriteIdx, _curTimerIdx, _curTextIdx;
    bool _skip, _blockSkip;

    private AsyncOperation _asyncLevelLoad;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx++];
        EmptyTexts();

        StartCoroutine(IntroSequence1());

        // async load insta loads and stalls the main thread for some reason -.-
        //_asyncLevelLoad = SceneManager.LoadSceneAsync("MainRiver");
        //_asyncLevelLoad.allowSceneActivation = false;
    }

    private IEnumerator LoadMainRiverAsync()
    {
        // async load insta loads and stalls the main thread for some reason -.-
        yield return new WaitForSeconds(1.0f);

        _asyncLevelLoad = SceneManager.LoadSceneAsync("MainRiver");
        _asyncLevelLoad.allowSceneActivation = false;

        while (!_asyncLevelLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator IntroSequence1()
    {
        yield return new WaitForSeconds(0.3f);

        _audioSource.PlayOneShot(_vignetteAudio1);
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _topTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _bottomTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx++];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _topTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _bottomTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx++];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _bottomTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx++];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI, 19.0f);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx++];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }
        EmptyTexts();
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI, 107.0f);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

    Next:
        if (_skip)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_vignetteAudio1_SignatureOnly);
        }
        EmptyTexts();
        _curSpriteIdx = 4;
        _curTextIdx = 8;
        _curTimerIdx = 16;
        _vignetteImage.sprite = _vignetteSprites[_curSpriteIdx];
        yield return null;

        StartCoroutine(RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI, 41.0f));
        StartCoroutine(AwaitSignature());
    }

    private IEnumerator AwaitSignature()
    {
        Debug.Log("Please sign");
        // TODO: Press space or mouse to proceed + indicator
        yield return null;

        while (true)
        {
            if (Input.GetButtonDown("DialogueNext"))
            {
                Debug.Log("Signed");
                EmptyTexts();
                _middleTextGUI.text = _vignetteTexts[8];
                break;
            }
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.PlayOneShot(_scribbleAudio, 5.0f);
        _vignetteImage.sprite = _vignetteSprites[5];

        yield return null;
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);

        StartCoroutine(IntroSequence2());
    }

    private IEnumerator IntroSequence2()
    {
        EmptyTexts();
        _audioSource.Stop();
        _audioSource.PlayOneShot(_vignetteAudio2);
        yield return null;

        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI);
        _middleTextGUI.text = _vignetteTexts[_curTextIdx - 1];
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }
        _vignetteImage.sprite = _vignetteSprites[3];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }


        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[6];
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _scrollTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _scrollTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        yield return RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _scrollTextGUI);
        if (_skip) { goto Next; }
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

        EmptyTexts();
        _vignetteImage.sprite = _vignetteSprites[5];
        StartCoroutine(RollText(_vignetteTexts[_curTextIdx++], _vignetteTimers[_curTimerIdx++], _middleTextGUI));
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }
        _vignetteImage.CrossFadeAlpha(0.0f, _vignetteTimers[_curTimerIdx++], false);
        yield return AwaitWithSkip(_vignetteTimers[_curTimerIdx++]);
        if (_skip) { goto Next; }

    Next:
        if (_skip)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_vignetteAudio2_OutroLaughOnly);
            EmptyTexts();
            _vignetteImage.sprite = _vignetteSprites[5];
            _vignetteImage.CrossFadeAlpha(0.0f, _vignetteTimers[_curTimerIdx++], false);

        }

        _blockSkip = true;
        yield return RollText(_vignetteTexts[15], _vignetteTimers[32], _bottomTextGUI);

        SceneManager.LoadScene("MainRiver");
        //_asyncLevelLoad.allowSceneActivation = true;
    }

    private void EmptyTexts()
    {
        _topTextGUI.text = String.Empty;
        _middleTextGUI.text = String.Empty;
        _bottomTextGUI.text = String.Empty;
        _scrollTextGUI.text = String.Empty;
    }

    private IEnumerator AwaitWithSkip(float time)
    {
        _skip = false;
        float timePassed = 0.0f;
        while (timePassed <= time)
        {
            timePassed += Time.deltaTime;
            if (Input.GetButtonDown("DialogueNext"))
            {
                _skip = true;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator RollText(string text, float time, TextMeshProUGUI textGUI)
    {
        _skip = false;
        float timer = 0.0f;
        float textWaitTime = time / text.Length;
        float textWaitTimer = textWaitTime;

        CharEnumerator charEnumerator = text.GetEnumerator();

        while (timer < time)
        {
            if (Input.GetButtonDown("DialogueNext") && !_blockSkip)
            {
                _skip = true;
                yield break;
            }

            timer += Time.deltaTime;
            textWaitTimer += Time.deltaTime;

            if (textWaitTimer >= textWaitTime)
            {
                textWaitTimer -= textWaitTime;

                if (charEnumerator.MoveNext())
                {
                    char c = charEnumerator.Current;
                    textGUI.text += c;

                    // Check for text formatting
                    if (c == '<')
                    {
                        while (charEnumerator.MoveNext())
                        {
                            c = charEnumerator.Current;
                            textGUI.text += c;

                            if (c == '>')
                            {
                                break;
                            }
                        }
                        if (charEnumerator.MoveNext())
                        {
                            textGUI.text += charEnumerator.Current;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    // Yeah it's bad, but I'm just doing a quick overhaul of the intro sequence and I don't care :p
    // - copy and pasted
    // - not accounting for multiple adjacent text formattings
    // - I made this quick workaround for when the text length doesn't match because it counts formatting characters
    private IEnumerator RollText(string text, float time, TextMeshProUGUI textGUI, float overrideTextLength)
    {
        _skip = false;
        float timer = 0.0f;
        float textWaitTime = time / overrideTextLength;
        float textWaitTimer = textWaitTime;

        CharEnumerator charEnumerator = text.GetEnumerator();

        while (timer < time)
        {
            if (Input.GetButtonDown("DialogueNext"))
            {
                _skip = true;
                yield break;
            }

            timer += Time.deltaTime;
            textWaitTimer += Time.deltaTime;

            if (textWaitTimer >= textWaitTime)
            {
                textWaitTimer -= textWaitTime;

                if (charEnumerator.MoveNext())
                {
                    char c = charEnumerator.Current;
                    textGUI.text += c;

                    // Check for text formatting
                    if (c == '<')
                    {
                        while (charEnumerator.MoveNext())
                        {
                            c = charEnumerator.Current;
                            textGUI.text += c;

                            if (c == '>')
                            {
                                break;
                            }
                        }
                        if (charEnumerator.MoveNext())
                        {
                            textGUI.text += charEnumerator.Current;
                        }
                    }
                }
            }
            yield return null;
        }
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSceneBox : MonoBehaviour
{
    [SerializeField] private Image _leftPortrait;
    [SerializeField] private Image _rightPortrait;
    [SerializeField] private TextMeshProUGUI _speakerLeftText;
    [SerializeField] private TextMeshProUGUI _speakerRightText;
    [SerializeField] private TextMeshProUGUI _LnameText;
    [SerializeField] private TextMeshProUGUI _RnameText;
    [SerializeField] private GameObject _textBoxLeft;
    [SerializeField] private GameObject _textBoxRight;
    [SerializeField] private GameObject _nextIndicatorL;
    [SerializeField] private GameObject _nextIndicatorR;

    // Controls how much faster the text is compared to the audio. This lets the text be a little ahead of the audio.
    private float _textLeadTime = 1.25f;

    private bool _isInitialDisplay = true;
    private bool _isRollingText;
    private float _textWaitTime;
    private float _textWaitTimer;
    private string lastSpeaker;

    private SpeakerSide _curSpeakerSide;
    private string _curText;
    private TextMeshProUGUI _curRollingtext;
    private CharEnumerator _curTextEnumerator;


    public void Display(string text, float playTime, SpeakerSide speakerSide,
        string speakerName, Sprite leftPortrait, Sprite rightPortrait, Color speakerColor)
    {
        gameObject.SetActive(true);

        if (speakerSide == SpeakerSide.None)
        {
            // SpeakerSide.None is not working at the moment, so we're falling back to SpeakerSide.Right
            speakerSide = SpeakerSide.Right;
        }

        switch (speakerSide)
        {
            case SpeakerSide.Left:
                _LnameText.text = speakerName;
                _LnameText.color = speakerColor;
                _curRollingtext = _speakerLeftText;
                break;
            // TODO: Would be nice to have a SpeakerSide.Right and then a new
            //       middle dialogue box for SpeakerSide.None/default
            case SpeakerSide.Right:
                _RnameText.text = speakerName;
                _RnameText.color = speakerColor;
                _curRollingtext = _speakerRightText;
                break;
        }
        _curRollingtext.text = "";


        if (_isInitialDisplay)
        {
            _isInitialDisplay = false;
            _curSpeakerSide = speakerSide;

            if (speakerSide == SpeakerSide.Left)
            {
                _textBoxLeft.GetComponent<Animator>().SetTrigger("boxMoveIn");
            }
            else if (speakerSide == SpeakerSide.Right)
            {
                _textBoxRight.GetComponent<Animator>().SetTrigger("boxMoveIn");
            }
        }
        else if (_curSpeakerSide != speakerSide)
        {
            switch (speakerSide)
            {
                case SpeakerSide.Left:
                    _textBoxLeft.GetComponent<Animator>().SetTrigger("boxMoveIn");
                    _textBoxRight.GetComponent<Animator>().SetTrigger("boxMoveOut");
                    _curSpeakerSide = speakerSide;
                    break;
                case SpeakerSide.Right:
                    _textBoxRight.GetComponent<Animator>().SetTrigger("boxMoveIn");
                    _textBoxLeft.GetComponent<Animator>().SetTrigger("boxMoveOut");
                    _curSpeakerSide = SpeakerSide.Right;
                    break;
            }
        }

        _leftPortrait.sprite = leftPortrait;
        _rightPortrait.sprite = rightPortrait;

        _nextIndicatorL.SetActive(false);
        _nextIndicatorR.SetActive(false);
        //_nextIndicator.SetActive(false);
        _textWaitTime = playTime / (text.Length * _textLeadTime);
        _curText = text;
        _curTextEnumerator = _curText.GetEnumerator();
        _isRollingText = true;
    }

    public string GetLastSpeaker()
    {
        return lastSpeaker;
    }

    public void Hide()
    {
        GetComponent<Animator>().SetTrigger("FinishScene");

        _isInitialDisplay = true;
        gameObject.SetActive(false);
        _isRollingText = false;
    }

    public void ShowAllText()
    {
        _curRollingtext.text = _curText;
        TextRollingDone();
    }

    public bool GetIsRollingText()
    {
        return _isRollingText;
    }

    private void RollText()
    {
        _textWaitTimer += Time.deltaTime;
        if (_textWaitTimer < _textWaitTime)
        {
            return;
        }
        _textWaitTimer = 0.0f;

        if (_curTextEnumerator.MoveNext())
        {
            char c = _curTextEnumerator.Current;
            _curRollingtext.text += c;

            // Check for text formatting
            if (c == '<')
            {
                while (_curTextEnumerator.MoveNext())
                {
                    c = _curTextEnumerator.Current;
                    _curRollingtext.text += c;

                    if (c == '>')
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            TextRollingDone();
        }
    }

    private void TextRollingDone()
    {
        if (_curSpeakerSide == SpeakerSide.Left)
        {
            _nextIndicatorL.SetActive(true);
        }
        else
        {
            _nextIndicatorR.SetActive(true);
        }

        _isRollingText = false;
    }

    private void Update()
    {
        if (_isRollingText)
        {
            RollText();
        }
    }
}

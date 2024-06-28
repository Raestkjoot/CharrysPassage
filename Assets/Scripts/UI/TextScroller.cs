using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextScroller : MonoBehaviour
{
 
    [SerializeField] private TextMeshProUGUI _text;
    
    [SerializeField] private GameObject _nextIndicator;

    // Controls how much faster the text is compared to the audio. This lets the text be a little ahead of the audio.
    private float _textLeadTime = 1.25f;

    private bool _isRollingText;
    private string _curText;
    private CharEnumerator _curTextEnumerator;
    private float _textWaitTime;
    private float _textWaitTimer;

    public void DisplayText(string text, float playTime)
    {
        _text.text = "";

        gameObject.SetActive(true);
        _nextIndicator.SetActive(false);

        _textWaitTime = playTime / (text.Length * _textLeadTime);
        _curText = text;
        _curTextEnumerator = _curText.GetEnumerator();
        _isRollingText = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _isRollingText = false;
    }

    public void ShowAllText()
    {
        _text.text = _curText;
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
            _text.text += c;

            // Check for text formatting
            if (c == '<')
            {
                while (_curTextEnumerator.MoveNext())
                {
                    c = _curTextEnumerator.Current;
                    _text.text += c;

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
        _nextIndicator.SetActive(true);
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

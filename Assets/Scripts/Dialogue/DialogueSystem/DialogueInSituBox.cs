using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInSituBox : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private TextMeshProUGUI _text;

    public void Display(string text, Sprite portrait)
    {
        _text.text = text;
        _portrait.sprite = portrait;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

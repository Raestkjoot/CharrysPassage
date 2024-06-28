using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueInSituLine", menuName = "Dialogue/DialogueInSituLine", order = 0)]
public class DialogueInSituLineSO : ScriptableObject
{
    [Tooltip("The text the character should be saying.")]
    [TextArea(1, 2)]
    [SerializeField] private string _text;
    [Tooltip("The voice over audio clip that should play with this dialogue line.")]
    [SerializeField] private AudioClip _audio;
    [Tooltip("What character is speaking? Used to select portrait and other relevant data.")]
    [SerializeField] private CharacterID _speaker;

    [Header("Overrides")]
    [Tooltip("Use to override the portrait. Leave empty to not override.")]
    [SerializeField] private Sprite _overridePortrait;
    [Tooltip("Use to override the length of time the dialogue is displayed. Leave equal to 0 to not override.")]
    [SerializeField] private float _overridePlayTime;


    public string GetText()
    {
        return _text;
    }

    public AudioClip GetAudio()
    {
        return _audio;
    }

    public CharacterID GetSpeaker()
    {
        return _speaker;
    }

    public float GetPlayTime()
    {
        if (_overridePlayTime > 0.001f)
        {
            return _overridePlayTime;
        }
        if (_audio != null)
        {

            return _audio.length; 
        }

        return _text.Length * 0.02f;
    }

    public Sprite GetPortrait()
    {
        return _overridePortrait;
    }
}
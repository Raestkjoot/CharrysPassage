using UnityEngine;

public enum SpeakerSide
{
    None = 0,
    Left,
    Right,
}

[CreateAssetMenu(fileName = "NewDialogueSceneLine", menuName = "Dialogue/DialogueSceneLine", order = 1)]
public class DialogueSceneLineSO : ScriptableObject
{
    [Tooltip("The text the character should be saying.")]
    [TextArea(5, 10)]
    [SerializeField] private string _text;
    [Tooltip("The voice over audio clip that should play with this dialogue line.")]
    [SerializeField] private AudioClip _audio;
    [Tooltip("What character is speaking? Left (Charry), Right (interlocutor).")]
    [SerializeField] private SpeakerSide _speaker;
    [Tooltip("Charry's conversation partner. The character on the right side of the dialogue box.")]
    [SerializeField] private CharacterID _interlocutor;

    [Header("Overrides")]
    [Tooltip("Use to override the portrait on the left side of the dialogue box. Leave empty to not override.")]
    [SerializeField] private Sprite _overridePortraitLeft;
    [Tooltip("Use to override the portrait on the right side of the dialogue box. Leave empty to not override.")]
    [SerializeField] private Sprite _overridePortraitRight;
    [Tooltip("Use to override the name of the speaker shown on the dialogue box. Leave empty to not override.")]
    [SerializeField] private string _overrideSpeakerName = "";
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

    public SpeakerSide GetSpeaker()
    {
        return _speaker;
    }

    public string GetOverrideSpeakerName()
    {
        return _overrideSpeakerName;
    }

    public CharacterID GetInterlocutor()
    {
        return _interlocutor;
    }

    public float GetPlayTime(ref bool isOverride)
    {
        if (_overridePlayTime > 0.001f)
        {
            isOverride = true;
            return _overridePlayTime;
        }
        
        if (_audio != null)
        {
            isOverride = false;
            return _audio.length; 
        }

        return _text.Length * 0.02f;
    }

    public Sprite GetLeftPortrait()
    {
        return _overridePortraitLeft;
    }

    public Sprite GetRightPortrait()
    {
        return _overridePortraitRight;
    }
}
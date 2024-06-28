using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : PersistentSingleton<DialogueManager>
{
    [SerializeField] private DialogueSceneBox _dialogueSceneBox;
    [SerializeField] private DialogueInSituBox _dialogueInSituBox;
    [SerializeField] private CharacterSO[] _characters;

    private AudioSource _audioSource;
    private bool _isPlayingDialogueInSitu = false;
    private int _currentPlayingPriority;

    private bool _isPlayingDialogueScene = false;
    private DialogueSceneLineSO[] _curSceneLines;
    private IEnumerator<DialogueSceneLineSO> _sceneLineEnumerator;
    private int _curPlayTimeID;
    private bool _blockSkip = false;

    private GameState previous = GameState.Gameplay;

    /// <summary>
    /// Play an in situ dialogue line if it fulfills the priority and chance requirements.
    /// </summary>
    /// <param name="line"> The dialogue line containing the text and other information. </param>
    /// <param name="priority"> A line with higher priority will interrupt lines with lower priority and be played instead. </param>
    /// <param name="chance"> The chance this line will be played. In the range 0.0f to 1.0f, where 1.0f means 100%. </param>
    public void PlayDialogueInSitu(DialogueInSituLineSO line, int priority = 0, float chance = 1.0f)
    {
#if UNITY_EDITOR
        if (DialogueDisabler.GetIsDialogueDisabled()) { return; }
#endif

        if (InSituCheck(priority, chance))
        {
            StartCoroutine(PlayLine(line));
        }
    }

    /// <summary>
    /// Play a sequence of in-situ dialogue lines one after another (if the sequence fulfills the priority and chance requirements).
    /// </summary>
    /// <param name="lines"> The collection of dialogue lines containing the text and other information. </param>
    /// <param name="priority"> A line with higher priority will interrupt lines with lower priority and be played instead. </param>
    /// <param name="chance"> The chance this line will be played. In the range 0.0f to 1.0f, where 1.0f means 100%. </param>
    public void PlayDialogueInSituSequence(DialogueInSituCollectionSO lines, int priority = 0, float chance = 1.0f)
    {
#if UNITY_EDITOR
        if (DialogueDisabler.GetIsDialogueDisabled()) { return; }
#endif

        if (InSituCheck(priority, chance))
        {
            StartCoroutine(PlayLines(lines.GetLines()));
        }
    }

    /// <summary>
    /// Select a random in-situ dialogue line from a collection and play it if the it fulfills the priority and chance requirements.
    /// </summary>
    /// <param name="lines"> The collection of dialogue lines containing the text and other information. </param>
    /// <param name="priority"> A line with higher priority will interrupt lines with lower priority and be played instead. </param>
    /// <param name="chance"> The chance this line will be played. In the range 0.0f to 1.0f, where 1.0f means 100%. </param>
    public void PlayDialogueInSituRandomSelect(DialogueInSituCollectionSO lines, int priority = 0, float chance = 1.0f)
    {
#if UNITY_EDITOR
        if (DialogueDisabler.GetIsDialogueDisabled()) { return; }
#endif

        if (InSituCheck(priority, chance))
        {
            StartCoroutine(PlayLine(lines.GetRandomLine()));
        }
    }

    /// <summary>
    /// Play a single dialogue scene line.
    /// </summary>
    /// <param name="line"> The dialogue line containing the text and other information. </param>
    public void PlayDialogueScene(DialogueSceneLineSO line)
    {
#if UNITY_EDITOR
        if (DialogueDisabler.GetIsDialogueDisabled()) { return; }
#endif

        previous = GameStateManager.Instance.CurrentGameState;
        GameStateManager.Instance.SetState(GameState.InCutscene);
        // Interrupt other dialogue if necessary
        if (_isPlayingDialogueInSitu)
        {
            _audioSource.Stop();
            _dialogueInSituBox.Hide();
            _isPlayingDialogueInSitu = false;
            StopAllCoroutines();
        }
        PlayLine(line);
    }


    /// <summary>
    /// Play a dialogue scene composed of a sequence of dialogue lines.
    /// </summary>
    /// <param name="lines"> The dialogue lines containing the text and other information. </param>
    public void PlayDialogueScene(DialogueSceneSO lines)
    {
#if UNITY_EDITOR
        if (DialogueDisabler.GetIsDialogueDisabled()) { return; }
#endif

        previous = GameStateManager.Instance.CurrentGameState;
        GameStateManager.Instance.SetState(GameState.InCutscene);
        // Interrupt other dialogue if necessary
        if (_isPlayingDialogueInSitu)
        {
            _audioSource.Stop();
            _dialogueInSituBox.Hide();
            _isPlayingDialogueInSitu = false;
            StopAllCoroutines();
        }
        PlayLines(lines.GetLines());
    }

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();

        if (_characters == null)
        {
            Debug.LogWarning("DialogueManager was not initialized properly.");
        }
    }

    private bool InSituCheck(int priority, float chance)
    {
        // Check priority and chance to see if we should skip this dialogue line.
        if (_isPlayingDialogueInSitu && priority <= _currentPlayingPriority)
        {
            return false;
        }
        if (chance < Random.Range(0.0f, 0.99f))
        {
            return false;
        }
        // Interrupt other dialogue if necessary
        if (_isPlayingDialogueInSitu)
        {
            _audioSource.Stop();
            StopAllCoroutines();
        }

        _currentPlayingPriority = priority;

        return true;
    }

    private void PlayLines(DialogueSceneLineSO[] lines)
    {
        _curSceneLines = lines;
        _sceneLineEnumerator = ((IEnumerable<DialogueSceneLineSO>)_curSceneLines).GetEnumerator();

        //The Enumerator here is used in ShowAllOrNext to 'finish' the while loop
        if (_sceneLineEnumerator.MoveNext())
        {
            PlayLine(_sceneLineEnumerator.Current);
        }
    }

    private IEnumerator PlayLines(DialogueInSituLineSO[] lines)
    {
        foreach (DialogueInSituLineSO line in lines)
        {
            yield return PlayLine(line);
        }
    }

    // The fundamental function for playing in-situ dialogue
    private IEnumerator PlayLine(DialogueInSituLineSO line)
    {
        _isPlayingDialogueInSitu = true;
        //string name = _characters[(int)line.GetSpeaker()].GetName();

        string text = line.GetText();
        Sprite portrait = _characters[(int)line.GetSpeaker()].GetInSituPortrait();

        _dialogueInSituBox.Display(text, portrait);

        AudioClip audioClip = line.GetAudio();
        if (audioClip)
        {
            _audioSource.PlayOneShot(line.GetAudio()); 
        }

        yield return new WaitForSeconds(line.GetPlayTime());

        _dialogueInSituBox.Hide();
        _isPlayingDialogueInSitu = false;
        _audioSource.Stop();
    }

    // The fundamental function for playing scene dialogue
    private void PlayLine(DialogueSceneLineSO line)
    {
        _isPlayingDialogueScene = true;

        SpeakerSide speaker = line.GetSpeaker();
        string speakerName = line.GetOverrideSpeakerName();
        Color speakerColor;

        // If speaker name is not overriden, get the speaker's name from character data
        if (speakerName == "")
        {
            speakerName = speaker switch
            {
                SpeakerSide.Left => _characters[1].GetName(),
                SpeakerSide.Right => _characters[(int)line.GetInterlocutor()].GetName(),
                _ => "None",
            };
        }

        speakerColor = speaker switch
        {
            SpeakerSide.Left => _characters[1].GetSceneColor(),
            SpeakerSide.Right => _characters[(int)line.GetInterlocutor()].GetSceneColor(),
            _ => Color.black ,
        };

        bool isPlayTimeOverride = false;
        float playTime = line.GetPlayTime(ref isPlayTimeOverride);

        _dialogueSceneBox.Display(
            line.GetText(),
            playTime,
            speaker,
            speakerName,
            GetPortrait(line, SpeakerSide.Left),
            GetPortrait(line, SpeakerSide.Right),
            speakerColor);

        AudioClip audioClip = line.GetAudio();
        if (audioClip)
        {
            _audioSource.PlayOneShot(line.GetAudio());
        }

        _curPlayTimeID++;
        if (isPlayTimeOverride)
        {
            StartCoroutine(OverridePlayTime(playTime, _curPlayTimeID));
        }
    }

    private Sprite GetPortrait(DialogueSceneLineSO line, SpeakerSide speaker)
    {
        Sprite sprite;

        if (speaker == SpeakerSide.Left)
        {
            sprite = line.GetLeftPortrait();
            if (sprite != null)
            {
                return sprite;
            }
            else
            {
                return _characters[(int)CharacterID.Charry].GetScenePortrait();
            }
        }
        else if (speaker == SpeakerSide.Right)
        {
            sprite = line.GetRightPortrait();
            if (sprite != null)
            {
                return sprite;
            }
            else
            {
                return _characters[(int)line.GetInterlocutor()].GetScenePortrait();
            }
        }

        return _characters[(int)CharacterID.None].GetScenePortrait();
    }

    private void ShowAllOrNext()
    {
        //Method is only called with space, this ignores that inputs when a DialogueScene is not playing
        if (!_isPlayingDialogueScene)
        {
            return;
        }

        if (_dialogueSceneBox.GetIsRollingText())
        {
            _dialogueSceneBox.ShowAllText();
        }
        else
        {
            if (_sceneLineEnumerator != null && _sceneLineEnumerator.MoveNext())
            {
                _audioSource.Stop();
                PlayLine(_sceneLineEnumerator.Current);
            }
            else
            {
                _dialogueSceneBox.Hide();
                _audioSource.Stop();
                _isPlayingDialogueScene = false;
                GameStateManager.Instance.SetState(previous);
            }
        }
    }

    IEnumerator OverridePlayTime(float playTime, int id)
    {
        yield return new WaitForSeconds(playTime);
        if (id == _curPlayTimeID)
        {
            _audioSource.Stop();
        }
    }

    private void Update()
    {
        if (!_isPlayingDialogueScene && Input.GetButtonDown("DialogueNext"))
        {
            _blockSkip = true;
        }

        if (Input.GetButtonUp("DialogueNext"))
        {
            if (_blockSkip)
            {
                _blockSkip = false;
            }
            else
            {
                ShowAllOrNext();
            }
        }
    }
}

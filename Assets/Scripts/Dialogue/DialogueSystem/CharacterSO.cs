using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Dialogue/CharacterData", order = 4)]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _scenePortrait;
    [SerializeField] private Sprite _inSituPortrait;
    [SerializeField] private Color _textColor;
    [SerializeField] private Sprite _dialogueSceneBox;
    [SerializeField] private Sprite _dialogueInSituBox;

    public string GetName()
    {
        return _name;
    }

    public Sprite GetScenePortrait()
    {
        return _scenePortrait;
    }

    public Sprite GetInSituPortrait()
    {
        return _inSituPortrait;
    }
    public Color GetSceneColor()
    {
        return _textColor;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueInSituCollection", menuName = "Dialogue/DialogueInSituCollection", order = 2)]
public class DialogueInSituCollectionSO : ScriptableObject
{
    [SerializeField] private DialogueInSituLineSO[] _dialogueInSituLines;

    public DialogueInSituLineSO[] GetLines()
    {
        return _dialogueInSituLines;
    }

    public DialogueInSituLineSO GetRandomLine()
    {
        int rnd = Random.Range(0, _dialogueInSituLines.Length);

        return _dialogueInSituLines[rnd];
    }
}

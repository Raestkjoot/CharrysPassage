using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueScene", menuName = "Dialogue/DialogueScene", order = 3)]
public class DialogueSceneSO : ScriptableObject
{
    [SerializeField] private DialogueSceneLineSO[] _dialogueSceneLines;

    public DialogueSceneLineSO[] GetLines()
    {
        return _dialogueSceneLines;
    }
}

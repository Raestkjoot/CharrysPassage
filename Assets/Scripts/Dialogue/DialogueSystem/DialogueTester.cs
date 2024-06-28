using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueInSituLineSO _inSituLine;
    [SerializeField] private DialogueInSituCollectionSO _inSituCollection;
    [SerializeField] private DialogueSceneLineSO _sceneLine;
    [SerializeField] private DialogueSceneSO _scene;

    public void TestDialogueInSitu()
    {
        DialogueManager.GetInstance().PlayDialogueInSitu(_inSituLine);
    }

    public void TestDialogueInSituPriority()
    {
        DialogueManager.GetInstance().PlayDialogueInSitu(_inSituLine, 1);
    }

    public void TestDialogueInSituChance()
    {
        DialogueManager.GetInstance().PlayDialogueInSitu(_inSituLine, 0, 0.3f);
    }

    public void TestDialogueInSituRandomSelect()
    {
        DialogueManager.GetInstance().PlayDialogueInSituRandomSelect(_inSituCollection);
    }

    public void TestDialogueInSituSequence()
    {
        DialogueManager.GetInstance().PlayDialogueInSituSequence(_inSituCollection);
    }

    public void TestDialogueScene()
    {
        DialogueManager.GetInstance().PlayDialogueScene(_sceneLine);
    }

    public void TestDialogueSceneSeq()
    {
        DialogueManager.GetInstance().PlayDialogueScene(_scene);
    }
}

using UnityEngine;

public class PlayInSituCollectionDialogueOnEnter : MonoBehaviour
{
    public enum PlayType
    {
        RandomSelect,
        Sequence
    }

    [SerializeField] private PlayType _type;
    [SerializeField] private DialogueInSituCollectionSO _dialogueCollection;
    [SerializeField] private int _priority = 0;
    [SerializeField] private float _chance = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_type == PlayType.Sequence)
            {
                DialogueManager.GetInstance().PlayDialogueInSituSequence(_dialogueCollection, _priority, _chance);
            }
            else if (_type == PlayType.RandomSelect)
            {
                DialogueManager.GetInstance().PlayDialogueInSituRandomSelect(_dialogueCollection, _priority, _chance);
            }
        }
    }
}

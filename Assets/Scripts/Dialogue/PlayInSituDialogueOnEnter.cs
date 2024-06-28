using UnityEngine;

public class PlayInSituDialogueOnEnter : MonoBehaviour
{
    [SerializeField] DialogueInSituLineSO _dialogueLine;
    [SerializeField] int _priority = 0;
    [SerializeField] float _chance = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.GetInstance().PlayDialogueInSitu(_dialogueLine, _priority, _chance);
        }
    }
}

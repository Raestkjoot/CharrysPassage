using UnityEngine;

public class PlayDialogueOnEnter : MonoBehaviour
{
    [SerializeField] DialogueSceneSO _dialogueScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.GetInstance().PlayDialogueScene(_dialogueScene);
        }
    }
}

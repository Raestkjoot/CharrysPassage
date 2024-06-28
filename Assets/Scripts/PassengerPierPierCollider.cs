using System.Collections;
using UnityEngine;

public class PassengerPierPierCollider : MonoBehaviour
{
    public enum PierType
    {
        PickUp,
        DropOff,
    }

    [SerializeField] private PierType _pierType;
    [SerializeField] private DialogueSceneSO _dialogueScene;

    [SerializeField] private float movePassengerDelay = 0.2f;
    [SerializeField] private float dialogueDelay = 0.2f;

    [SerializeField] private GameObject _standingPassenger;


    private bool _hasInteracted = false;

    public bool GetHasInteracted()
    {
        return _hasInteracted;
    }

    private void Start()
    {
        if (_pierType == PierType.PickUp)
        {
            _standingPassenger.SetActive(true);
        }
        else if (_pierType == PierType.DropOff)
        {
            _standingPassenger.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovementLanes>(out PlayerMovementLanes boat))
        {
            StartCoroutine(InteractWithPassenger(boat));
        }
    }

    private IEnumerator InteractWithPassenger(PlayerMovementLanes boat)
    {
        yield return new WaitForSeconds(movePassengerDelay);

        if (_pierType == PierType.PickUp)
        {
            _standingPassenger.SetActive(false);
            boat.AddPassenger();
        }
        else if (_pierType == PierType.DropOff)
        {
            boat.RemovePassenger();
            _standingPassenger.SetActive(true);
        }

        yield return new WaitForSeconds(dialogueDelay);

        if (_dialogueScene != null)
        {
            DialogueManager.GetInstance().PlayDialogueScene(_dialogueScene);
        }

        boat.ResumeMovement();
        _hasInteracted = true;
    }
}

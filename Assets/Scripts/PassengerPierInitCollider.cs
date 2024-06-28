using UnityEngine;

public class PassengerPierInitCollider : MonoBehaviour
{
    [SerializeField] private PassengerPierPierCollider _passengerPierPierCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_passengerPierPierCollider.GetHasInteracted() == false)
        {
            if (collision.gameObject.TryGetComponent<PlayerMovementLanes>(out PlayerMovementLanes boat))
            {
                boat.PauseMovement();
            } 
        }
    }
}

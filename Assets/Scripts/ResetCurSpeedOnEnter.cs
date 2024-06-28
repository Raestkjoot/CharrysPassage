using UnityEngine;

public class ResetCurSpeedOnEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovementLanes>(out PlayerMovementLanes boat))
        {
            boat.ResetCurrentSpeed();
        }
    }
}

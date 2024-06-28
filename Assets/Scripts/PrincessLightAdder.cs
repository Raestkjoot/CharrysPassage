using UnityEngine;

public class PrincessLightAdder : MonoBehaviour
{
    [SerializeField] private GameObject _princessLightPrefab;
    [SerializeField] private bool _isAdder;

    static private GameObject _princessLightGameObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isAdder)
            {
                _princessLightGameObject = Instantiate(_princessLightPrefab, other.transform);
            }
            else
            {
                Destroy(_princessLightGameObject);
            }
                
        }
    }
}

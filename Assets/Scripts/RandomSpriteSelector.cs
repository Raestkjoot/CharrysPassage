using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
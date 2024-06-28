using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private static int _collectableCount = 0;
    [SerializeField] private AudioClip[] _soundEffects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _collectableCount++;
            int rnd = Random.Range(0, _soundEffects.Length);

            AudioSource.PlayClipAtPoint(_soundEffects[rnd], transform.position);

            Destroy(gameObject);
        }
    }

    public int GetCollectableCount()
    {
        return _collectableCount;
    }
  
}
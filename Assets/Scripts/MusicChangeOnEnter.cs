using UnityEngine;

public class MusicChangeOnEnter : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private float _FadeOutDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicManager.GetInstance().ChangeMusic(_musicClip, _FadeOutDuration);
        }
    }
}

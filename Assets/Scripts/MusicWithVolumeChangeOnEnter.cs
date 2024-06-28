using UnityEngine;

public class MusicWithVolumeChangeOnEnter : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private float _FadeOutDuration = 0.5f;
    [SerializeField] private float _NewVolume = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicManager.GetInstance().ChangeMusic(_musicClip, _NewVolume, _FadeOutDuration);
        }
    }
}

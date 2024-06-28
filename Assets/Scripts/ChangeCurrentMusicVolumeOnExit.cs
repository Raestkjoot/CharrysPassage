using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentMusicVolumeOnExit : MonoBehaviour
{
    [SerializeField] private float _volume = 0.3f;
    [SerializeField] private float _fadeDuration = 1.0f;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.GetInstance().ChangeVolumeForCurrentTrackTo(_volume, _fadeDuration);
            /*Console.WriteLine("Here");
            Debug.Log("here again");*/
        }
    }

}

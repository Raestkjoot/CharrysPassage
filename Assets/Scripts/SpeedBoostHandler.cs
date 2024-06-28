using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostHandler : MonoBehaviour
{
    [SerializeField] private float _speedBoostPower = 1.0f;
    [SerializeField] private AudioClip _soundEffect;
    public ParticleSystem speedBoostParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementLanes playerMovement = other.GetComponent<PlayerMovementLanes>();
            if (playerMovement != null)
            {
                playerMovement.Accelerate(_speedBoostPower);
                var emission = speedBoostParticles.emission;
                emission.enabled = true;
                speedBoostParticles.Play();

                AudioSource.PlayClipAtPoint(_soundEffect, transform.position);
            }
        }
    }
}

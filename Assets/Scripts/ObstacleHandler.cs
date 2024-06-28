using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] private float _obstaclePower = 2.0f;
    [SerializeField] private AudioClip _soundEffect;
    [SerializeField] private GameObject _sprite;
    public ParticleSystem obstacleParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementLanes playerMovement = other.GetComponent<PlayerMovementLanes>();
            if (playerMovement != null)
            {
                var emission = obstacleParticles.emission;
                emission.enabled = true;
                obstacleParticles.Play();
                Destroy(_sprite);

                playerMovement.Accelerate(-_obstaclePower);

                AudioSource.PlayClipAtPoint(_soundEffect, transform.position);
                Destroy(this);
            }
        }
    }
}
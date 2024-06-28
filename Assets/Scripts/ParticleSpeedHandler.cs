using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedHandler : MonoBehaviour
{
    public ParticleSystem frontParticles, backParticles; 
    public PlayerMovementLanes playerMovement;

    public Gradient colorGradient;

    void Update()
    {
        float emissionRate = CalculateEmissionRate(playerMovement.GetCurForwardSpeed());

        var emissionModuleFront = frontParticles.emission;
        var emissionModuleBack = backParticles.emission;
        emissionModuleFront.rateOverTime = emissionRate;
        emissionModuleBack.rateOverTime = emissionRate;

        MoveWithPlayer();
        SetColorGradient();
    }

    private float CalculateEmissionRate(float playerSpeed)
    {
        float minSpeed = 0.02f; 
        float maxSpeed = 0.05f;

        playerSpeed = Mathf.Clamp(playerSpeed, minSpeed, maxSpeed);

        float minEmissionRate = 0f; 
        float maxEmissionRate = 100f; 

        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, (playerSpeed - minSpeed) / (maxSpeed - minSpeed));

        return emissionRate;
    }

    private void MoveWithPlayer()
    {
        // Move particle system to player's position
        transform.position = playerMovement.transform.position;
    }

    private void SetColorGradient()
    {
        var colorOverLifetimeFront = frontParticles.colorOverLifetime;

        var colorOverLifetimeBack = backParticles.colorOverLifetime;
        colorOverLifetimeFront.color = colorGradient;
        colorOverLifetimeBack.color = colorGradient;
    }
}

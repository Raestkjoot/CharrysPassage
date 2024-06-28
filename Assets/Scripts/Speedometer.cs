using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _minSpeed = 0f;
    [SerializeField] private Scrollbar _speedometerSlider;
    private PlayerMovementLanes _playerMovementLanes;

    private void Start()
    {
        // Needs to be set here, since the speedometer is instantiated into the main river scene when 
        // we get to the Ziggy River. We can't use SerializeField.
        _playerMovementLanes = GameObject.FindWithTag("Player").GetComponent<PlayerMovementLanes>();
    }

    private void Update()
    {
        float curSpeed = _playerMovementLanes.GetCurForwardSpeed();
        float normalizedSpeed = Mathf.InverseLerp(_minSpeed, _maxSpeed, curSpeed);

        _speedometerSlider.value = normalizedSpeed;
    }
}

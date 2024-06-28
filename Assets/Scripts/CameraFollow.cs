using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _boat;
    [SerializeField] private float _smoothness = 0.18f;
    [SerializeField] private float _maxForwardOffset = 20.0f;
    [SerializeField] private float _forwardOffsetSpeed = 1.5f;
    [SerializeField] private float _maxZoomOut = 6.0f;
    [SerializeField] private float _zoomOutSpeed = 1.0f;

    private PlayerMovementLanes _boatMovement;
    private Transform _target;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _delta;
    private float _staticYPos;

    private float _startOffset;
    private float _startZoom;
    private float _boatNormalSpeed;

    private void Start()
    {
        _target = new GameObject("CameraTarget").transform;
        _target.parent = _boat.transform;
        _target.localPosition = Vector3.zero;

        _delta = transform.position - _target.position;
        _staticYPos = transform.position.y;

        _startOffset = _target.localPosition.x;
        _startZoom = _target.localPosition.z;
        _maxZoomOut = -_maxZoomOut;

        _boatMovement = _boat.GetComponent<PlayerMovementLanes>();
        _boatNormalSpeed = _boatMovement.GetNormalForwardSpeed();
    }

    private void LateUpdate()
    {
        float boatSpeed = _boatMovement.GetCurForwardSpeed() - _boatNormalSpeed;
        // calculate new forward offset
        float targetOffset = Mathf.Lerp(_startOffset, _maxForwardOffset, boatSpeed);
        float currentOffset = _target.localPosition.x;
        float newOffset = Mathf.Lerp(currentOffset, targetOffset, _forwardOffsetSpeed * Time.deltaTime);
        // calculate new zoom out
        float targetZoom = Mathf.Lerp(_startZoom, _maxZoomOut, boatSpeed);
        float currentZoom = _target.localPosition.z;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, _zoomOutSpeed * Time.deltaTime);
        // update offset and zoom on target
        _target.localPosition = new Vector3(newOffset, _target.localPosition.y, newZoom);

        // move camera towards target
        Vector3 targetPosition = _target.position + _delta;
        targetPosition.y = _staticYPos;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothness);
    }
}
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovementLanes : MonoBehaviour
{
    public enum Direction
    {
        None = 0,
        Left = 1,
        Right = -1,
    }

    [Header("Forward movement")]
    [SerializeField] private float _normalForwardSpeed = 0.075f;
    [SerializeField] private float _acceleration = 0.4f;
    [SerializeField] private float _drag = 0.2f;

    [Header("Lane movement")]
    [SerializeField] private float _SideMoveMultiplierTimeThreshold = 0.2f;
    [SerializeField] private float _sideMoveCooldown = 1.0f;
    [SerializeField] private float _sideSpeed = 0.2f;
    [SerializeField] private int _laneCount = 5;
    [SerializeField] private float _laneWidth = 2.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip _idleAudio;
    [SerializeField] private AudioClip _chargeAudio;
    [SerializeField] private AudioClip _paddleAudio;
    private AudioSource _audioSource;

    [SerializeField] private Animator _animator;

    private float _curForwardSpeed = 0.02f;
    private float _lastSideMoveTime = -1.0f;
    private float _lastStoredActionTime = -1.0f;
    private float _sidePosition;
    private Vector2 _minMaxLaneSidepPos;
    private bool _cancelAction = false;

    private Rigidbody2D _rigidBody;
    private Direction _currentAction;
    private Transform _kagebunshinBoat;

    // For passenger pier
    private float _saveNormalSpeed;

    public void Accelerate(float power)
    {
        _curForwardSpeed += power;

        if (_curForwardSpeed < 0.0f)
        {
            _curForwardSpeed = 0.0f;
        }
    }

    public float GetCurForwardSpeed()
    {
        return _curForwardSpeed;
    }

    public float GetNormalForwardSpeed()
    {
        return _normalForwardSpeed;
    }

    public void ResetCurrentSpeed()
    {
        _curForwardSpeed = _normalForwardSpeed;
    }

    public void PauseMovement()
    {
        _curForwardSpeed = 0.0f;
        _normalForwardSpeed = 0.0f;
    }

    public void ResumeMovement()
    {
        _normalForwardSpeed = _saveNormalSpeed;
    }

    public void AddPassenger()
    {
        PlayAnimation("HasPassenger", true);
    }
    public void RemovePassenger()
    {
        PlayAnimation("HasPassenger", false);
    }

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _sidePosition = _rigidBody.position.y;

        float deltaSidePos = (_laneCount / 2) * _laneWidth;
        _minMaxLaneSidepPos = new Vector2(_sidePosition - deltaSidePos, _sidePosition + deltaSidePos);
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        _kagebunshinBoat = transform.Find("BoatShadowSprite");
        if (_kagebunshinBoat)
        {
            _kagebunshinBoat.GetComponent<SpriteRenderer>().enabled = false;
            _kagebunshinBoat.parent = null;
        }
        else
        {
            Debug.LogWarning("Shadow boat missing: Did not find gameobject with name 'BoatShadowSprite' in boat's children.");
        }

        _saveNormalSpeed = _normalForwardSpeed;
    }

    private void Update()
    {
        if (Input.GetButton("PaddleLeft"))
        {
            TryStoreAction(Direction.Left);
        }
        if (Input.GetButton("PaddleRight"))
        {
            TryStoreAction(Direction.Right);
        }

        if (Input.GetButtonUp("PaddleLeft"))
        {
            TryExecuteAction(Direction.Left);
        }
        if (Input.GetButtonUp("PaddleRight"))
        {
            TryExecuteAction(Direction.Right);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // change in speed. If slower than normal speed: accelerate. Else: apply drag*(speed^2).
        float delta = (_curForwardSpeed < _normalForwardSpeed) ?
            _acceleration * Time.deltaTime :
            _drag * _curForwardSpeed * _curForwardSpeed * Time.deltaTime;

        _curForwardSpeed = Mathf.Lerp(_curForwardSpeed, _normalForwardSpeed, delta);

        float xTargetPos = _rigidBody.position.x + _curForwardSpeed;
        float yTargetPos = Mathf.Lerp(_rigidBody.position.y, _sidePosition, _sideSpeed);
        _rigidBody.MovePosition(new Vector2(xTargetPos, yTargetPos));

        //TODO: Idle audio + animation
        PlayAudio(_idleAudio);
        PlayAnimation("Idle", true);
    }

    private void TryStoreAction(Direction direction)
    {
        //TODO: Charge audio + animation
        PlayAudio(_chargeAudio);
        PlayAnimation("Paddle", 100); // So we don't immediately paddle
        PlayAnimation("Charge", true);

        if (Time.time < _lastSideMoveTime + _sideMoveCooldown) return;
        if (_cancelAction == true) return;


        if (_currentAction == Direction.None)
        {
            _currentAction = direction;
            _lastStoredActionTime = Time.time;
        }
        else if (_currentAction != direction)
        {
            _currentAction = Direction.None;
            _cancelAction = true;
            _kagebunshinBoat.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        UpdateShadowBoat();
    }

    private void TryExecuteAction(Direction direction)
    {
        if (_cancelAction == true)
        {
            _cancelAction = false;
            _lastStoredActionTime = Time.time;
            return;
        }

        float _accumulatedTime = Time.time - _lastStoredActionTime;

        SideMove((int)((int)direction * (_accumulatedTime / _SideMoveMultiplierTimeThreshold)));

        _lastStoredActionTime = Time.time;
        _currentAction = Direction.None;

        if (_kagebunshinBoat != null)
        {
            _kagebunshinBoat.GetComponent<SpriteRenderer>().enabled = false; 
        }
    }

    private void SideMove(int delta)
    {
        PlayAudio(_paddleAudio);
        PlayAnimation("Paddle", math.clamp(delta, -4, 4));
        PlayAnimation("Charge", false);
        PlayAnimation("Idle", true); // So we return to idle after the paddle animation

        if (Time.time >= _lastSideMoveTime + _sideMoveCooldown)
        {
            _sidePosition += _laneWidth * delta;
            _sidePosition = Mathf.Clamp(_sidePosition, _minMaxLaneSidepPos.x, _minMaxLaneSidepPos.y);
            _lastSideMoveTime = Time.time;
        }
    }

    private void UpdateShadowBoat()
    {
        if (_kagebunshinBoat == null) return;

        float accTime = Time.time - _lastStoredActionTime;
        int shadowLaneDelta = (int)_currentAction * (int)(accTime / _SideMoveMultiplierTimeThreshold);

        if (shadowLaneDelta != 0)
        {
            _kagebunshinBoat.GetComponent<SpriteRenderer>().enabled = true;

            float newPosY = _sidePosition + shadowLaneDelta * _laneWidth;
            newPosY = Mathf.Clamp(newPosY, _minMaxLaneSidepPos.x, _minMaxLaneSidepPos.y);

            _kagebunshinBoat.position = new Vector3(transform.position.x,
                newPosY, _kagebunshinBoat.position.z);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        _currentAction = Direction.None;
        _kagebunshinBoat.GetComponent<SpriteRenderer>().enabled = false;
        enabled = newGameState == GameState.Gameplay;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    private void PlayAnimation(string animName, int chargeLevel)
    {
        if (_animator != null)
        {
            _animator.SetInteger(animName, chargeLevel);
        }
    }

    private void PlayAnimation(string animName, bool newValue)
    {
        if (_animator != null)
        {
            _animator.SetBool(animName, newValue);
        }
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
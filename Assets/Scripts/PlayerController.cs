using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private enum STATE
    {
        NORMAL,
    }

    private STATE _state = STATE.NORMAL;

    private Transform _transform;
    private Rigidbody _rigidbody;

    private Vector2 _direction;
    private float _roll;

    [SerializeField] private float _rollspeed = 0.1f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _deceleration = 0.15f;
    [SerializeField] private float _maxAccelerationSpeed = 5f;

    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _rotationdeceleration = 1f;

    private Vector2 _mouseVel;
    private Vector3 _veldamp = Vector3.zero;
    private Vector3 _rotdamp = Vector3.zero;

    Vector2 rotation = Vector2.zero;

    [SerializeField] private AudioSource _shootSource;

    private bool _holdingSlide = false;
    [SerializeField] private Transform _legPivot;

    [SerializeField] private TextMeshProUGUI _debugText1;

    private void Awake()
    {
        Instance = this;
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _debugText1.text = "NOT SLIDING";
        _legPivot.gameObject.SetActive(false);

        _mouseVel += Mouse.current.delta.ReadValue();
        switch (_state)
        {
            case STATE.NORMAL:
                if (_holdingSlide) SlideCheck();
                break;
        }
        
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case STATE.NORMAL:
                Movement();
                RotationVel();
                MovementDeceleration();
                RotationDeceleration();
                
                break;
        }

        _mouseVel = Vector3.zero;
    }

    private void Movement()
    {
        if (Vector3.Dot(_rigidbody.velocity, transform.forward * _direction.y) < _maxAccelerationSpeed)
        {
            _rigidbody.velocity += _transform.forward * _direction.y * _acceleration * Time.fixedDeltaTime;
        }
        if (Vector3.Dot(_rigidbody.velocity, transform.right * _direction.x) < _maxAccelerationSpeed)
        {
            _rigidbody.velocity += _transform.right * _direction.x * _acceleration * Time.fixedDeltaTime;
        }
    }

    private void RotationVel()
    {
        float Pitch = -_mouseVel.y * _mouseSensitivity;
        float Yaw = _mouseVel.x * _mouseSensitivity;
        Vector3 torqueY = _transform.up * Yaw;
        Vector3 torqueX = _transform.right * Pitch;
        Vector3 torqueZ = _transform.forward * _roll * _rollspeed;

        _rigidbody.AddTorque(torqueY, ForceMode.VelocityChange);
        _rigidbody.AddTorque(torqueX, ForceMode.VelocityChange);
        _rigidbody.AddTorque(torqueZ, ForceMode.VelocityChange);

        /*rotationX += -Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        _transform.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * _mouseSensitivity, Input.GetAxis("Mouse X") * _mouseSensitivity, 0);*/
    }

    private void MovementDeceleration()
    {
        _rigidbody.velocity -= _deceleration * _rigidbody.velocity.normalized * Time.fixedDeltaTime;
    }
    private void RotationDeceleration()
    {
        //_rigidbody.angularVelocity -= _rigidbody.angularVelocity.normalized * _rotationdeceleration;
        _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, Vector3.zero, _rotationdeceleration * Time.fixedDeltaTime);
    }

    private void SlideCheck()
    {
        foreach (Collider item in Physics.OverlapSphere(_transform.position, 2, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            Vector3 itemPoint = item.ClosestPoint(_transform.position);
            if (Physics.Raycast(_transform.position, (_transform.forward - itemPoint).normalized, out RaycastHit hitInfo, 100, ~(1 << LayerMask.NameToLayer("Player"))))
            {
                Vector3 newDirection = Vector3.ProjectOnPlane(_rigidbody.velocity, hitInfo.normal).normalized;
                if (Vector3.Dot(newDirection, _rigidbody.velocity.normalized) > 0.75f)
                {
                    _rigidbody.velocity = newDirection * _rigidbody.velocity.magnitude;
                    _legPivot.gameObject.SetActive(true);
                    _legPivot.LookAt((_transform.position + _rigidbody.velocity), hitInfo.normal);
                    _debugText1.text = "IS SLIDING";
                    break;
                }
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _holdingSlide = true;
        }
        else if (context.canceled)
        {
            _holdingSlide = false;
        }
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _rigidbody.velocity += transform.forward * 10;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _direction = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            _direction = Vector2.zero;
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _roll = -context.ReadValue<Vector2>().x;
        }
        if (context.canceled)
        {
            _roll = 0;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _rotationdeceleration = 1f;

    private Vector2 _mouseVel;
    private Vector3 _veldamp = Vector3.zero;
    private Vector3 _rotdamp = Vector3.zero;

    Vector2 rotation = Vector2.zero;

    [SerializeField] private AudioSource _shootSource;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _mouseVel += Mouse.current.delta.ReadValue();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _shootSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _shootSource.Stop();
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
        _rigidbody.velocity += _transform.forward * _direction.y * _acceleration * Time.fixedDeltaTime;
        _rigidbody.velocity += _transform.right * _direction.x * _acceleration * Time.fixedDeltaTime;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrapplingGun : Weapon
{
    [Header("Grappling Hook")]
    [SerializeField] float _grapplingForce;
    [SerializeField] float _grapplingMaxForce;
    [SerializeField] float _grapplingSideBrakeForce;

    private RaycastHit _grapplingHitInfo;
    private bool b_grappling = false;
    private bool b_swinging = false;
    private float _oldHookDistance;

    private LineRenderer _lineRenderer;
    private Transform _canonPosition;

    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = _meshTransform.Find("Line").GetComponent<LineRenderer>();
        _canonPosition = _meshTransform.Find("CanonPosition").transform;
    }

    protected override void Update()
    {
        _lineRenderer.transform.position = Vector3.zero;
        _lineRenderer.transform.eulerAngles = Vector3.zero;
        if (b_grappling || b_swinging)
        {
            transform.LookAt(_grapplingHitInfo.point);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            _lineRenderer.SetPosition(0, _canonPosition.position);
            _lineRenderer.SetPosition(1, _grapplingHitInfo.point);
        }
        else
        {
            _lineRenderer.SetPosition(0, _canonPosition.position);
            _lineRenderer.SetPosition(1, _canonPosition.position);
            CalculateWeaponRotationOffset();
        }

        
    }

    private void FixedUpdate()
    {
        if (b_grappling)
        {
            Color grapplingColor = new Color();
            grapplingColor.r = 32f / 255; grapplingColor.g = 53f / 255; grapplingColor.b = 115f / 255; grapplingColor.a = 1;
            _lineRenderer.startColor = grapplingColor;
            _lineRenderer.endColor = grapplingColor;
            float forwardSpeed = Vector3.Dot(_ownerRigidbody.velocity, transform.forward);
            Vector3 otherDirectionsSpeed = _ownerRigidbody.velocity - (forwardSpeed * transform.forward);
            if (forwardSpeed < _grapplingMaxForce)
            {
                _ownerRigidbody.AddForce(_grapplingForce * transform.forward, ForceMode.VelocityChange);
            }
            if (forwardSpeed < 0)
            {
                _ownerRigidbody.AddForce(-_ownerRigidbody.velocity.normalized * _grapplingSideBrakeForce, ForceMode.VelocityChange);
            }
            else
            {
                _ownerRigidbody.AddForce(-otherDirectionsSpeed.normalized * _grapplingSideBrakeForce, ForceMode.VelocityChange);
            }
        }
        else if (b_swinging)
        {
            Color swingingColor = new Color();
            swingingColor.r = 115f / 255; swingingColor.g = 47f / 255; swingingColor.b = 0f / 255; swingingColor.a = 1;
            _lineRenderer.startColor = swingingColor;
            _lineRenderer.endColor = swingingColor;
            Vector3 grappleDirection = (_grapplingHitInfo.point - transform.position);
            float grappleDistance = grappleDirection.magnitude;
            float velocity = _ownerRigidbody.velocity.magnitude;
            Vector3 newDirection = Vector3.ProjectOnPlane(_ownerRigidbody.velocity, grappleDirection.normalized);
            if (grappleDistance >= _oldHookDistance)
            {
                _ownerRigidbody.velocity = newDirection.normalized * velocity;
            }
            else
            {

            }


            _oldHookDistance = grappleDistance;
        }
    }

    public override void Shoot()
    {
        if (b_swinging)
        {
            b_swinging = false;
            b_grappling = true;
        }
        else if (CanShoot())
        {
            if (Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
            {
                _oldHookDistance = _maxRange + 999;

                _ammo--;

                if (_impactParticles)
                {
                    Destroy(Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal)), 10);
                }
                b_grappling = true;
                _grapplingHitInfo = hitInfo;
            }

            if (_shootParticles)
            {
                Destroy(Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation), 10);
            }
        }
    }

    public override void StopShooting()
    {
        if (b_grappling)
        {
            b_grappling = false;
            StartCoroutine(WaitForFireRate());
        }
    }

    public override void AlternateShoot()
    {
        if (b_grappling)
        {
            b_grappling = false;
            b_swinging = true;
        }
        else
        {
            if (CanShoot())
            {
                if (Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
                {
                    _oldHookDistance = _maxRange + 999;

                    _ammo--;

                    if (_impactParticles)
                    {
                        Destroy(Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal)), 10);
                    }
                    b_swinging = true;
                    _grapplingHitInfo = hitInfo;
                }

                if (_shootParticles)
                {
                    Destroy(Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation), 10);
                }
            }
        }
    }

    public override void StopAlternateShooting()
    {
        if (b_swinging)
        {
            b_swinging = false;
        }
    }
}

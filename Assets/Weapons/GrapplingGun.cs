using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : Weapon
{
    [Header("Grappling Hook")]
    [SerializeField] float _grapplingForce;
    [SerializeField] float _grapplingMaxForce;

    private RaycastHit grapplingHitInfo;
    private bool b_grappling = false;

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
        base.Update();
        _lineRenderer.transform.position = Vector3.zero;
        _lineRenderer.transform.eulerAngles = Vector3.zero;
        if (b_grappling)
        {
            transform.LookAt(grapplingHitInfo.point);
            _lineRenderer.SetPosition(0, _canonPosition.position);
            _lineRenderer.SetPosition(1, grapplingHitInfo.point - _lineRenderer.transform.position);

            _ownerRigidbody.AddForce(_ownKnockback * -transform.forward, ForceMode.VelocityChange);
        }
        else
        {
            _lineRenderer.SetPosition(0, _canonPosition.position);
            _lineRenderer.SetPosition(1, _canonPosition.position);
        }
    }

    public override void Shoot()
    {
        if (CanShoot())
        {
            _ammo--;

            if (Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
            {
                if (_impactParticles)
                {
                    Destroy(Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal)), 10);
                }
                b_grappling = true;
                grapplingHitInfo = hitInfo;
            }
            
            StartCoroutine(WaitForFireRate());

            if (_shootParticles)
            {
                Destroy(Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation), 10);
            }
        }
    }

    public override void StopShooting()
    {
        b_grappling = false;
    }
}

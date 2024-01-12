using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    private Camera _cam;

    private bool b_aiming = false;
    private float _baseFov;

    [Header("Sniper")]
    [SerializeField] private float _aimingFov;

    private void Start()
    {
        _cam = Camera.main;
        _baseFov = _cam.fieldOfView;
    }

    protected override void Update()
    {
        base.Update();

        _meshTransform.gameObject.SetActive(!b_aiming);
        HUDScript.Instance.Crosshair.gameObject.SetActive(!b_aiming);
        HUDScript.Instance.SniperCrosshair.gameObject.SetActive(b_aiming);
        if (b_aiming)
        {
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _aimingFov, 12 * Time.deltaTime);
        }
        else
        {
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _baseFov, 12 * Time.deltaTime);
            if (!b_hasSwitched)
            {
                _cam.fieldOfView = _baseFov;
            }
        }

        if (b_isReloading || !b_hasSwitched)
        {
            b_aiming = false;
        }
    }

    public override void AlternateShoot()
    {
        if (CanShoot(false) || b_aiming)
        {
            b_aiming = !b_aiming;
        }
    }
}

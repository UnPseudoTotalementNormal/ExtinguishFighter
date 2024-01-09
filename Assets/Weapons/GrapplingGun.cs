using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : Weapon
{
    public override void Shoot()
    {
        if (CanShoot())
        {
            _ammo--;

            if (Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
            {
                _ownerRigidbody.AddForce(_ownKnockback * -_ownerRigidbody.transform.forward, ForceMode.VelocityChange);

                if (_impactParticles)
                {
                    Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal));
                }
                if (hitInfo.collider.transform.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
                {
                    healthComponent.ModifyHealth(-_damage);
                }
            }
            
            StartCoroutine(WaitForFireRate());

            if (_shootParticles)
            {
                Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation);
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Shoot()
    {
        if (CanShoot())
        {
            _ammo--;

            if (Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo,_maxRange))
            {
                if (_impactParticles)
                {
                    Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal));
                }
                if (hitInfo.collider.transform.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
                {
                    healthComponent.ModifyHealth(-_damage);
                }
            }

            _ownerRigidbody.AddForce(_ownKnockback * -_ownerRigidbody.transform.forward, ForceMode.VelocityChange);
            StartCoroutine(WaitForFireRate());

            if (_shootParticles)
            {
                Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation);
            }
        }
    }
}

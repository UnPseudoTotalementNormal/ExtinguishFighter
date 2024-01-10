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
                    Destroy(Instantiate(_impactParticles, hitInfo.point, Quaternion.Euler(hitInfo.normal)), 10);
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
                Destroy(Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation), 10);
            }

            transform.localRotation *= Quaternion.Euler(new Vector3(-_recoilVisualForce, 0, 0));
        }
    }
}

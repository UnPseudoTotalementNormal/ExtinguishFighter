using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Shoot()
    {
        if (CanShoot())
        {
            _ammo--;

            _ownerRigidbody.AddForce(_ownKnockback * -_ownerRigidbody.transform.forward, ForceMode.VelocityChange);
            StartCoroutine(WaitForFireRate());

            if (_shootParticles)
            {
                Instantiate(_shootParticles, GetParticlesTransform().position, _shootParticles.transform.rotation * GetParticlesTransform().rotation);
            }
        }
    }
}

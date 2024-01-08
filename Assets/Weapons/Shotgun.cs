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
        }
    }
}

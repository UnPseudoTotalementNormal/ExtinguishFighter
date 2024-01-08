using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Shoot()
    {
        if (CanShoot())
        {
            _ownerRigidbody.AddForce(_ownKnockback * -_ownerRigidbody.transform.forward);
        }
    }
}

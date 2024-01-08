using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected private Rigidbody _ownerRigidbody;
    protected enum BULLET_TYPE
    {
        RAYCAST,
        PROJECTILE,
    }

    protected enum SHOOT_TYPE
    {
        AUTOMATIC,
        SEMI_AUTOMATIC,
    }

    [Header("Type")]
    [SerializeField] protected private BULLET_TYPE _bulletType;
    [SerializeField] protected private SHOOT_TYPE _shootType;

    [Header("Ammo")]
    [SerializeField] protected private int _ammo;
    [SerializeField] protected private int _maxAmmo;
    [SerializeField] protected private float _reloadTime;

    [Header("Shoot")]
    [SerializeField] protected private float _damage;
    [SerializeField] protected private float _impactKnocback;
    [SerializeField] protected private float _ownKnockback;
    [SerializeField] protected private float _firingRate;

    [Header("Misc")]
    [SerializeField] protected private float _switchTime;
    [SerializeField] protected private float _unswitchTime;


    protected private bool b_isReloading = false;
    protected private bool b_waitingFireRate = false;
    protected private bool b_hasSwitched = true;

    public bool HasSwitched { get { return b_hasSwitched; } }

    public virtual IEnumerator Switching()
    {
        yield return null;
    }

    public abstract void Shoot();

    public virtual bool CanShoot()
    {
        return (b_hasSwitched && !b_isReloading && !b_waitingFireRate);
    }

    public virtual IEnumerator Reload()
    {
        b_isReloading = true;
        yield return new WaitForSeconds(_reloadTime);
        b_isReloading = false;
    }

    public virtual IEnumerator WaitForFireRate()
    {
        b_waitingFireRate = true;
        yield return new WaitForSeconds(_firingRate);
        b_waitingFireRate = false;
    }

    public virtual IEnumerator Unswitching()
    {
        yield return null;
    }
}

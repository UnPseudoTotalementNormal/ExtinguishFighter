using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
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
    [SerializeField] protected private float _maxRange;
    [SerializeField] protected private float _impactKnocback;
    [SerializeField] protected private float _ownKnockback;
    [SerializeField] protected private float _firingRate;
    [SerializeField] protected private GameObject _shootParticles;
    [SerializeField] protected private GameObject _impactParticles;

    [Header("Misc")]
    [SerializeField] protected private float _switchTime;
    [SerializeField] protected private float _unswitchTime;

    [Header("Sfx")]
    [SerializeField] protected private AudioClip _shootSound;
    [SerializeField] protected private AudioClip _reloadSound;


    protected private bool b_isReloading = false;
    protected private bool b_waitingFireRate = false;
    protected private bool b_hasSwitched = true;

    public int Ammo { get { return _ammo; } }
    public int MaxAmmo { get { return _maxAmmo; } }
    public bool HasSwitched { get { return b_hasSwitched; } }
    public float SwitchTime { get { return _switchTime; } }
    public float UnswitchTime {  get { return _unswitchTime; } }

    public virtual IEnumerator Switching()
    {
        gameObject.SetActive(true);
        b_hasSwitched = false;
        yield return new WaitForSeconds(SwitchTime);
        b_hasSwitched = true;
    }

    public virtual IEnumerator Unswitching()
    {
        b_hasSwitched = false;
        yield return new WaitForSeconds(UnswitchTime);
        b_hasSwitched = true;
        gameObject.SetActive(false);
    }

    public virtual void Shoot()
    {
        if (CanShoot())
        {
            _ammo--;

            if (_bulletType == BULLET_TYPE.RAYCAST && Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
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

    public virtual bool CanShoot(bool needAmmo = true)
    {
        if (needAmmo)
        {
            return (_ammo > 0 && b_hasSwitched && !b_isReloading && !b_waitingFireRate);
        }
        else
        {
            return (b_hasSwitched && !b_isReloading && !b_waitingFireRate);
        }
        
    }

    public virtual IEnumerator Reload()
    {
        if (CanShoot(false) && _ammo != _maxAmmo)
        {
            b_isReloading = true;
            yield return new WaitForSeconds(_reloadTime);
            b_isReloading = false;
            _ammo = _maxAmmo;
        }
    }

    public virtual IEnumerator WaitForFireRate()
    {
        b_waitingFireRate = true;
        yield return new WaitForSeconds(_firingRate);
        b_waitingFireRate = false;
    }

    public Transform GetParticlesTransform()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("ParticlesPosition"))
            {
                return child;
            }
        }
        return null;
    }
}

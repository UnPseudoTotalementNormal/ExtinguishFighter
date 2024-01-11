using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] protected private float _ownKnockbackTorque;
    [SerializeField] protected private float _firingRate;
    [SerializeField] protected private float _recoilVisualForce;
    [SerializeField] protected private GameObject _shootParticles;
    [SerializeField] protected private GameObject _impactParticles;

    [Header("Misc")]
    [SerializeField] protected private float _switchTime;
    [SerializeField] protected private float _unswitchTime;

    [Header("Sfx")]
    [SerializeField] protected private AudioClip _shootSound;
    [SerializeField, Range(0, 1)] protected private float _shootVolume = 1;
    [SerializeField] protected private AudioClip _reloadSound;
    [SerializeField, Range(0, 1)] protected private float _reloadVolume = 1;
    [SerializeField] protected private AudioSource _holdingSound;
    [SerializeField, Range(0, 1)] protected private float _holdingVolume = 1;
    [SerializeField] protected private AudioClip _switchingSound;
    [SerializeField, Range(0, 1)] protected private float _switchingVolume = 1;
    [SerializeField] protected private AudioClip _unswitchingSound;
    [SerializeField, Range(0, 1)] protected private float _unswitchingVolume = 1;

    protected private bool b_automaticShooting = false;
    protected private bool b_isReloading = false;
    protected private bool b_waitingFireRate = false;
    protected private bool b_hasSwitched = true;

    protected private Vector3 _weaponRotationOffset = Vector3.zero;
    protected private Transform _meshTransform;

    public int Ammo { get { return _ammo; } }
    public int MaxAmmo { get { return _maxAmmo; } }
    public bool HasSwitched { get { return b_hasSwitched; } }
    public float SwitchTime { get { return _switchTime; } }
    public float UnswitchTime {  get { return _unswitchTime; } }

    protected virtual void Awake()
    {
        _meshTransform = transform.Find("Mesh");
    }

    protected virtual void Update()
    {
        CalculateWeaponRotationOffset();
        if (b_automaticShooting && _shootType == SHOOT_TYPE.AUTOMATIC)
        {
            Shoot();
        }
    }

    protected virtual private void CalculateWeaponRotationOffset()
    {
        if (b_isReloading || !b_hasSwitched)
        {
            Transform switchingOffset = transform.Find("SwitchingOffset");
            if (switchingOffset) TransformToOffset(switchingOffset);
        }
        else
        {
            Transform defaultOffset = transform.Find("DefaultOffset");
            if (defaultOffset) TransformToOffset(defaultOffset);
        }

        _weaponRotationOffset = _ownerRigidbody.angularVelocity * 2;
        _meshTransform.localRotation = Quaternion.Lerp(_meshTransform.localRotation, Quaternion.Euler(_weaponRotationOffset), 6);
    }
    protected private void TransformToOffset(Transform tOffset, bool b_lerp = true, float t = 12)
    {
        if (b_lerp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, tOffset.localPosition, t * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, tOffset.localRotation, t * Time.deltaTime);
        }
        else
        {
            transform.localPosition = tOffset.localPosition;
            transform.localRotation = tOffset.localRotation;
        }
    }

    public virtual IEnumerator Switching()
    {
        Transform switchingOffset = transform.Find("SwitchingOffset");
        if (switchingOffset) TransformToOffset(switchingOffset, false);

        gameObject.SetActive(true);

        CustomAudio newAudio = new CustomAudio();
        newAudio.AudioClip = _switchingSound;
        newAudio.Volume = _switchingVolume;
        SoundSystem.Instance.Play(newAudio);
        
        if (_holdingSound) { _holdingSound.Play(); _holdingSound.volume = _holdingVolume; }

        b_hasSwitched = false;
        yield return new WaitForSeconds(SwitchTime);
        b_hasSwitched = true;
    }

    public virtual IEnumerator Unswitching()
    {
        CustomAudio newAudio = new CustomAudio();
        newAudio.AudioClip = _unswitchingSound;
        newAudio.Volume = _unswitchingVolume;
        SoundSystem.Instance.Play(newAudio);

        b_hasSwitched = false;
        yield return new WaitForSeconds(UnswitchTime);
        b_hasSwitched = true;
        gameObject.SetActive(false);

        if (_holdingSound) _holdingSound.Stop();
    }

    public virtual void Shoot()
    {
        b_automaticShooting = true;
        if (CanShoot())
        {
            _ammo--;

            if (_bulletType == BULLET_TYPE.RAYCAST && Physics.Raycast(_ownerRigidbody.transform.position, _ownerRigidbody.transform.forward, out RaycastHit hitInfo, _maxRange))
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
            _ownerRigidbody.AddTorque(-_ownKnockbackTorque * _ownerRigidbody.transform.right, ForceMode.VelocityChange);

            CustomAudio newAudio = new CustomAudio();
            newAudio.AudioClip = _shootSound;
            newAudio.Volume = _shootVolume;
            newAudio.b_RandomPitch = true;
            SoundSystem.Instance.Play(newAudio);
        }
    }

    public virtual void StopShooting() 
    { 
        b_automaticShooting = false;
    }

    public virtual void AlternateShoot() { }

    public virtual void StopAlternateShooting() { }

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
            CustomAudio newAudio = new CustomAudio();
            newAudio.AudioClip = _reloadSound;
            newAudio.Volume = _reloadVolume;
            SoundSystem.Instance.Play(newAudio);

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

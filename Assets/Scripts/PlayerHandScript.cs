using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandScript : MonoBehaviour
{
    public static PlayerHandScript Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeactivateAllWeapons();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        SwitchWeaponWithHotkey();
    }

    private void DeactivateAllWeapons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var childI = transform.GetChild(i);
            if (childI.gameObject.activeSelf && childI.TryGetComponent<Weapon>(out Weapon weaponComponent))
            {
                childI.gameObject.SetActive(false);
            }
        }
        return;
    }

    public Weapon GetCurrentWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var childI = transform.GetChild(i);
            if (childI.gameObject.activeSelf && childI.TryGetComponent<Weapon>(out Weapon weaponComponent))
            {
                return weaponComponent;
            }
        }
        return null;
    }

    private void SwitchWeaponWithHotkey()
    {
        Weapon currentWeapon = GetCurrentWeapon();
        if (currentWeapon && currentWeapon.HasSwitched && currentWeapon.CanShoot(false))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(SwitchWeapon(0));
            if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(SwitchWeapon(1));
            if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(SwitchWeapon(2));
            if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(SwitchWeapon(3));
            if (Input.GetKeyDown(KeyCode.Alpha5)) StartCoroutine(SwitchWeapon(4));
            if (Input.GetKeyDown(KeyCode.Alpha6)) StartCoroutine(SwitchWeapon(5));
            if (Input.GetKeyDown(KeyCode.Alpha7)) StartCoroutine(SwitchWeapon(6));
            if (Input.GetKeyDown(KeyCode.Alpha8)) StartCoroutine(SwitchWeapon(7));
            if (Input.GetKeyDown(KeyCode.Alpha9)) StartCoroutine(SwitchWeapon(8));
            if (Input.GetKeyDown(KeyCode.Alpha0)) StartCoroutine(SwitchWeapon(9));
        }
    }

    private IEnumerator SwitchWeapon(int toWeaponIndex)
    {
        Weapon weaponSwitched = GetCurrentWeapon();
        Weapon weaponSwitchingTo = transform.GetChild(toWeaponIndex).GetComponent<Weapon>();
        StartCoroutine(weaponSwitched.Unswitching());
        while (!weaponSwitched.HasSwitched)
        {
            yield return null;
        }
        StartCoroutine(weaponSwitchingTo.Switching());
        yield return null;
    }

    public void OnWeaponSwitchRoll(InputAction.CallbackContext context)
    {
        if (context.started && (int)context.ReadValue<Vector2>().y != 0)
        {
            Weapon currentWeapon = GetCurrentWeapon();
            Transform currentWeaponTransform = currentWeapon.transform;
            if (currentWeapon.HasSwitched && currentWeapon.CanShoot(false))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (currentWeaponTransform == transform.GetChild(i))
                    {
                        if (transform.childCount <= (i + (int)context.ReadValue<Vector2>().y) || (i + (int)context.ReadValue<Vector2>().y) < 0)
                        {
                            if (i + (int)context.ReadValue<Vector2>().y < 0)
                            {
                                StartCoroutine(SwitchWeapon(transform.childCount - 1));
                            }
                            else
                            {
                                StartCoroutine(SwitchWeapon(0));
                            }
                        }
                        else
                        {
                            StartCoroutine(SwitchWeapon(i + (int)context.ReadValue<Vector2>().y));
                        }
                        return;
                    }
                }
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Weapon weapon = GetCurrentWeapon();
        if (weapon)
        {
            if (context.started)
            {
                weapon.Shoot();
            }
            if (context.canceled)
            {
                weapon.StopShooting();
            }
        }
        
    }

    public void OnAlternateShoot(InputAction.CallbackContext context)
    {
        Weapon weapon = GetCurrentWeapon();
        if (weapon)
        {
            if (context.started)
            {
                weapon.AlternateShoot();
            }
            if (context.canceled)
            {
                weapon.StopAlternateShooting();
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Weapon weapon = GetCurrentWeapon();
            if (weapon)
            {
                StartCoroutine(weapon.Reload());
            }
        }
    }
}

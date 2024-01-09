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
        if (context.started)
        {
            Weapon weapon = GetCurrentWeapon();
            if (weapon) 
            {
                weapon.Shoot();
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

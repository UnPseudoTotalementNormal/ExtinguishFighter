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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GetCurrentWeapon().Shoot();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine(GetCurrentWeapon().Reload());
        }
    }
}

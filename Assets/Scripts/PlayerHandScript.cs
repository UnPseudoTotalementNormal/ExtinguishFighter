using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandScript : MonoBehaviour
{
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childI = transform.GetChild(i);
                if (childI.gameObject.activeSelf && childI.TryGetComponent<Weapon>(out Weapon weaponComponent))
                {
                    weaponComponent.Shoot();
                }
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childI = transform.GetChild(i);
                if (childI.gameObject.activeSelf && childI.TryGetComponent<Weapon>(out Weapon weaponComponent))
                {
                    StartCoroutine(weaponComponent.Reload());
                }
            }
        }
    }
}

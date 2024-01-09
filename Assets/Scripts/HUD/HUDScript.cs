using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    private PlayerHandScript _playerHandScript;

    [SerializeField] private TextMeshProUGUI _ammoText;

    private void Start()
    {
        _playerHandScript = PlayerHandScript.Instance;
    }

    private void Update()
    {
        AmmoDisplay();
    }

    private void AmmoDisplay()
    {
        Weapon currentWeapon = _playerHandScript.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            _ammoText.text = "Ammo: " + currentWeapon.Ammo + "/" + currentWeapon.MaxAmmo;
        }
    }
}

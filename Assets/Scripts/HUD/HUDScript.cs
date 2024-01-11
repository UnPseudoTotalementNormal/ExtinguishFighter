using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public static HUDScript Instance;

    private PlayerHandScript _playerHandScript;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _weaponText;
    [SerializeField] private TextMeshProUGUI _speedText;

    [SerializeField] private Image _crosshair;
    [SerializeField] private Image _sniperCrosshair;

    public Image Crosshair { get { return _crosshair; } }
    public Image SniperCrosshair { get { return _sniperCrosshair; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerHandScript = PlayerHandScript.Instance;
    }

    private void Update()
    {
        AmmoDisplay();
        _weaponText.alpha = Mathf.Lerp(_weaponText.alpha, 0, 1 * Time.deltaTime);
        _speedText.text = ((int)(PlayerController.Instance.GetComponent<Rigidbody>().velocity.magnitude*5)).ToString() + " km/h";

        Weapon currentWeapon = PlayerHandScript.Instance.GetCurrentWeapon();
        if (currentWeapon && _weaponText.text != currentWeapon.name)
        {
            _weaponText.text = currentWeapon.name;
            _weaponText.alpha = 1;
        }
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

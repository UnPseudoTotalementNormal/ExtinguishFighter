using UnityEngine;


[CreateAssetMenu(fileName ="New Weapon", menuName ="Weapon/Create New Weapon")]
public class WeaponObject : ScriptableObject
{
    public GameObject Prefab;
    public int Ammo;
    public int MaxAmmo;
}

using UnityEngine;

public class Musket : Weapon
{
    [Header("Musker")]
    [SerializeField] private Transform _notMovingPivot;

    protected override void Update()
    {
        base.Update();
        _notMovingPivot.localRotation = Quaternion.Inverse(transform.localRotation);
    }
}

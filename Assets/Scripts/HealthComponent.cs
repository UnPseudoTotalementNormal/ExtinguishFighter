using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float _health;
    [SerializeField] private float _maxHealth;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void ModifyHealth(float add)
    {
        _health = Mathf.Clamp(_health + add, 0, _maxHealth);
        if ( _health <= 0) 
        {
            Destroy(gameObject);
        }
    }
}

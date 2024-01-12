using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private bool b_deathToRigidbody = false;
    private void Awake()
    {
        _health = _maxHealth;
    }

    public void ModifyHealth(float add)
    {
        _health = Mathf.Clamp(_health + add, 0, _maxHealth);
        if ( _health <= 0) 
        {
            TryGetComponent<Rigidbody>(out Rigidbody rb);
            if (!b_deathToRigidbody )
            {
                Destroy(gameObject);
            }
            else if (!rb)
            {
                transform.AddComponent<Rigidbody>();
            }
        }
    }
}

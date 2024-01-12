using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    public static EnemyCount Instance;

    private float _enemyNumber = 0;
    private float _enemyKilled = 0;

    public float EnemyNumber { get { return _enemyNumber; } }
    public float EnemyKilled {  get { return _enemyKilled; } }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddEnemyNumber()
    {
        _enemyNumber++;
    }

    public void AddEnemyKilled()
    {
        _enemyKilled++;
    }
}

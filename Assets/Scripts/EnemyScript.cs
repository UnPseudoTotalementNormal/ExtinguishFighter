using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private void Start()
    {
        EnemyCount.Instance.AddEnemyNumber();
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            EnemyCount.Instance.AddEnemyKilled();
        }
    }
}

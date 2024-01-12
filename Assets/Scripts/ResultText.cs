using TMPro;
using UnityEngine;

public class ResultText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private void Start()
    {
        timerText.text = "Secondes: " + CheckPointsHandler.Instance.Timer.ToString("0.00") + "\n" 
            + "Enemy killed: " + EnemyCount.Instance.EnemyKilled + "/" + EnemyCount.Instance.EnemyNumber;
    }
}

using TMPro;
using UnityEngine;

public class ResultText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private void Start()
    {
        if (PlayerPrefs.GetFloat("Timer")  == 0f)
        {
            PlayerPrefs.SetFloat("Timer", 9999999);
        }
        float timeDone = CheckPointsHandler.Instance.Timer;
        float enemyKilled = EnemyCount.Instance.EnemyKilled;
        print(PlayerPrefs.GetFloat("Timer"));
        print(PlayerPrefs.GetFloat("Enemy"));
        if ((timeDone < PlayerPrefs.GetFloat("Timer") && enemyKilled >= PlayerPrefs.GetFloat("EnemyKilled")) || (enemyKilled > PlayerPrefs.GetFloat("EnemyKilled")))
        {
            SaveStats(timeDone, enemyKilled);
        }
        timerText.text = "Secondes: " + timeDone.ToString("0.00") + "\n" 
            + "Enemy killed: " + enemyKilled + "/" + EnemyCount.Instance.EnemyNumber + "\n" +
            "PB sec: " + PlayerPrefs.GetFloat("Timer").ToString("0.00") + "\n" +
            "PB enemy killed: " + PlayerPrefs.GetFloat("EnemyKilled");

    }

    public void SaveStats(float time, float enemyKilled)
    {
        PlayerPrefs.SetFloat("Timer", time);
        PlayerPrefs.SetFloat("EnemyKilled", enemyKilled);
    }
}

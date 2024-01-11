using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance;

    private float _timer = 0;

    private bool b_isPlaying = true;

    public float Timer {  get { return _timer; } }

    private void Awake()
    {
        Instance = this;
        b_isPlaying = true;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void RestartTimer()
    {
        _timer = 0;
    }

    public void StopTimer()
    {
        b_isPlaying = false;
    }

    public void PlayTimer()
    {
        b_isPlaying = true;
    }
}

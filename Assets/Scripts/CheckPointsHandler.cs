using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsHandler : MonoBehaviour
{
    public static CheckPointsHandler Instance;

    [SerializeField] private List<CheckPoint> _checkPoints = new List<CheckPoint>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var checkPoint in _checkPoints)
        {
            checkPoint.gameObject.SetActive(false);
        }
        _checkPoints[0].gameObject.SetActive(true);
    }

    public void EnteredCheckPoint(CheckPoint checkPoint)
    {
        int checkPointIndex = GetCheckPointIndex(checkPoint);
        if (checkPointIndex == _checkPoints.Count-1)
        {
            print(LevelTimer.Instance.Timer);
        }
        else
        {
            checkPoint.gameObject.SetActive(false);
            _checkPoints[checkPointIndex+1].gameObject.SetActive(true);
        }
    }

    public bool CheckAllCheckPointsCleared()
    {
        int checkPointsCleared = 0;
        for (int i = 0;  i < _checkPoints.Count; i++)
        {
            if (_checkPoints[i].Cleared)
            {
                checkPointsCleared++;
            }
        }

        return (checkPointsCleared >= _checkPoints.Count);
    }

    private int GetCheckPointIndex(CheckPoint checkPoint)
    {
        return _checkPoints.IndexOf(checkPoint);
    }
}

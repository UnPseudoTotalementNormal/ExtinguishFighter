using UnityEngine;

public class HelpArrow : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    void Update()
    {
        CheckPoint checkPoint = CheckPointsHandler.Instance.GetCurrentCheckpoint();
        if (checkPoint)
        {
            _pivot.LookAt(checkPoint.transform.position);
        }
    }
}

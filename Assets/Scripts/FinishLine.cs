using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print(LevelTimer.Instance.Timer);
        }
    }
}

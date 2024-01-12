using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool _cleared = false;

    public bool Cleared { get { return _cleared; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointsHandler.Instance.EnteredCheckPoint(this);
            _cleared = true;
        }
    }
}

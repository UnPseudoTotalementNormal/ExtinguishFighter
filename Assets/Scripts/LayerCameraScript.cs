using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCameraScript : MonoBehaviour
{
    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
         _cam.fieldOfView = Camera.main.fieldOfView;
    }
}

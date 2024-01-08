using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 1.5f;

    [SerializeField] private Transform _pivotX;
    [SerializeField] private Transform _pivotY;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerCamera();
    }

    private void PlayerCamera()
    {
        float rotX = -Input.GetAxis("Mouse Y");
        float rotY = Input.GetAxis("Mouse X");
        _pivotX.eulerAngles += new Vector3(rotX * _mouseSensitivity, 0);
        //_pivotY.eulerAngles += new Vector3(0, rotY * _mouseSensitivity);
        _transform.eulerAngles += new Vector3(0, rotY * _mouseSensitivity);
    }
}

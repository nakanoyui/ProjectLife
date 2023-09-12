using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController
/// </summary>
public class PlayerController : MonoBehaviour
{
    private float _speed = 5.0f;

    private void Update()
    {
        Vector3 moveDir = ((transform.forward * JoyStick._joyStickY) + (transform.right * JoyStick._joyStickX)).normalized;

        transform.position += moveDir * _speed * Time.deltaTime;
    }
}

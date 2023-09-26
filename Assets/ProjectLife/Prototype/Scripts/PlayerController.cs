using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Serialization;

/// <summary>
/// PlayerController
/// </summary>
public class PlayerController : MonoBehaviour
{
    private float _speed = 5.0f;

    private void Start()
    {
        OnEvent();
    }

    private void Update()
    {
        Move();
    }

    private void OnEvent()
    {
    }

    private void Move()
    {
        var joyStickDir = new Vector3(JoyStick.JoyStickX, 0,JoyStick.JoyStickY).normalized;
        
        transform.position += joyStickDir * _speed * Time.deltaTime;

        if (joyStickDir.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(joyStickDir);
        }
    }
}
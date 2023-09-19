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
    [SerializeField]
    private GameObject _itemPrefabs;

    [SerializeField]
    private ActionButton _actionButton;
    
    private float _speed = 5.0f;

    private bool _isCatchFish;

    public bool IsCatchFish
    {
        get { return _isCatchFish;}
        set { _isCatchFish = value; }
    }

    private void Start()
    {
        OnEvent();
    }

    private void Update()
    {
        Vector3 moveDir = ((transform.forward * JoyStick.JoyStickY) + (transform.right * JoyStick.JoyStickX)).normalized;

        transform.position += moveDir * _speed * Time.deltaTime;
    }

    private void OnEvent()
    {
        _actionButton.OnClickButton.Subscribe(_ =>
        {
            _itemPrefabs.SetActive(true);


            if (_isCatchFish)
            {
                Debug.Log("魚を釣りあげた。");
            }
        });
    }
}

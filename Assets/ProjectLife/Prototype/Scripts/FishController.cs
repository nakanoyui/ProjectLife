using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Serialization;

/// <summary>
/// FishController
/// </summary>
public class FishController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    
    [SerializeField] private Transform _rotTransform;

    [SerializeField] private ActionButton _actionButton;

    private Transform _targetTransform;

    private float _speed = 0.01f;

    private void Start()
    {
        OnEvent();
    }

    private void Update()
    {
        if (_targetTransform != null)
        {
            var distance = _targetTransform.position - transform.position;

            if (distance.sqrMagnitude < 0.01f)
            {
                _targetTransform = null;
                _playerController.IsCatchFish = true;
            }
            
            transform.position += distance * _speed;
        }
    }

    private void OnEvent()
    {
        _actionButton.OnClickButton.Subscribe(_ =>
        {
            _targetTransform = _rotTransform;
        });
    }
}
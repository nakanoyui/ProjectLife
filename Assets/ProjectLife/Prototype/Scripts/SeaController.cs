using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class SeaController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private LureController _lureController;
    
    [SerializeField] private ActionButton _actionButton;

    [SerializeField] private GameObject _fishPrefab;
    
    private GameObject _currentFish;

    private Subject<bool> _onExistFish = new();

    private bool _is;
    
    private void Start()
    {
        OnEvent();
    }

    private void Update()
    {
        if (_currentFish == null)
        {
            _onExistFish.OnNext(true);
        }
    }

    private void OnEvent()
    {
        _onExistFish.Subscribe(_ =>
        {
            _currentFish = Instantiate(_fishPrefab, transform);
            var fishBrain = _currentFish.GetComponent<FishBrain>();
            fishBrain.Init(_playerTransform, _lureController, _actionButton);
            _lureController.FishBrain = fishBrain; 
        });
    }
}

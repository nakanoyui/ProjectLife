using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// ActionButton
/// </summary>
public class ActionButton : Button
{
    public enum ActionButtonState
    {
        None,
        Cast,
        Fish,
    }
    
    private ActionButtonState _currentState = ActionButtonState.Cast;

    public ActionButtonState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    
    private Subject<float> _onClickCastButton = new();
    private Subject<bool> _onClickFishButton = new();
    
    public IObservable<float> OnClickCastButton { get { return _onClickCastButton; } }
    public IObservable<bool> OnClickFishButton { get { return _onClickFishButton; } }

    private float _holdPower;
    
    private const float HoldPowerMax = 3.0f;
    
    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        OnButtonHold.Subscribe(_ =>
        {
            if (_holdPower < HoldPowerMax)
            {
                _holdPower += Time.deltaTime;
            }
            else
            {
                switch (_currentState)
                {
                    case ActionButtonState.Cast:
                        _onClickCastButton.OnNext(_holdPower);
                        _currentState = ActionButtonState.Fish;
                        _holdPower = 0.0f;
                        break;
                }
            }
        });

        OnButtonDown.Subscribe(_ =>
        {
            switch (_currentState)
            {
                case ActionButtonState.Fish:
                    _onClickFishButton.OnNext(true);
                    _currentState = ActionButtonState.None;
                    break;
            }
        });
        
        OnButtonUp.Subscribe(_ =>
        {
            switch (_currentState)
            {
                case ActionButtonState.Cast:
                    _onClickCastButton.OnNext(_holdPower);
                    _currentState = ActionButtonState.Fish;
                    _holdPower = 0.0f;
                    break;
            }
        });
    }
}

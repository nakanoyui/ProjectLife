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
        FishingBattle,
    }
    
    private ActionButtonState _currentState = ActionButtonState.Cast;

    public ActionButtonState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    
    private Subject<bool> _onClickCastButton = new();
    private Subject<bool> _onClickFishButton = new();
    private Subject<bool> _onHoldFishingBattleButton = new();
    private Subject<bool> _onReleaseFishingBattleButton = new();
    
    public IObservable<bool> OnClickCastButton { get { return _onClickCastButton; } }
    public IObservable<bool> OnClickFishButton { get { return _onClickFishButton; } }
    public IObservable<bool> OnHoldFishingBattleButton { get { return _onHoldFishingBattleButton; } }
    public IObservable<bool> OnReleaseFishingBattleButton { get { return _onReleaseFishingBattleButton; } }
    
    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        OnButtonHold.Subscribe(_ =>
        {
            switch (_currentState)
            {
                case ActionButtonState.FishingBattle:
                    _onHoldFishingBattleButton.OnNext(true);
                    break;
            }
        });

        OnButtonDown.Subscribe(_ =>
        {
            switch (_currentState)
            {
                case ActionButtonState.Cast:
                    _onClickCastButton.OnNext(true);
                    _currentState = ActionButtonState.Fish;
                    break;  
                case ActionButtonState.Fish:
                    _onClickFishButton.OnNext(true);
                    _currentState = ActionButtonState.None;
                    break;
            }
        });
        
        OnButtonUp.Subscribe(_ =>
        {
        });
        
        OnButtonRelease.Subscribe(_ =>
        {
            switch (_currentState)
            {
                case ActionButtonState.FishingBattle:
                    _onReleaseFishingBattleButton.OnNext(true);
                    break;
            }
        });
    }
}

using System;
using UniRx;

/// <summary>
///     ActionButton
/// </summary>
public class ActionButton : Button
{
    public enum ActionButtonState
    {
        None,
        Cast,
        Fish,
        FishingBattle,
        PlantsWatering
    }

    private readonly Subject<bool> _onClickCastButton = new();
    private readonly Subject<bool> _onClickFishButton = new();
    private readonly Subject<bool> _onHoldFishingBattleButton = new();

    private readonly Subject<bool> _onPlantsWateringButton = new();
    private readonly Subject<bool> _onReleaseFishingBattleButton = new();

    public ActionButtonState CurrentState { get; set; } = ActionButtonState.None;

    public IObservable<bool> OnClickCastButton => _onClickCastButton;
    public IObservable<bool> OnClickFishButton => _onClickFishButton;
    public IObservable<bool> OnHoldFishingBattleButton => _onHoldFishingBattleButton;
    public IObservable<bool> OnReleaseFishingBattleButton => _onReleaseFishingBattleButton;

    public IObservable<bool> OnPlantsClickWateringButton => _onPlantsWateringButton;

    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        OnButtonHold.Subscribe(_ =>
        {
            switch (CurrentState)
            {
                case ActionButtonState.FishingBattle:
                    _onHoldFishingBattleButton.OnNext(true);
                    break;
            }
        });

        OnButtonDown.Subscribe(_ =>
        {
            switch (CurrentState)
            {
                case ActionButtonState.Cast:
                    _onClickCastButton.OnNext(true);
                    CurrentState = ActionButtonState.Fish;
                    break;
                case ActionButtonState.Fish:
                    _onClickFishButton.OnNext(true);
                    CurrentState = ActionButtonState.None;
                    break;
                case ActionButtonState.PlantsWatering:
                    _onPlantsWateringButton.OnNext(true);
                    CurrentState = ActionButtonState.PlantsWatering;
                    break;
            }
        });

        OnButtonUp.Subscribe(_ => { });

        OnButtonRelease.Subscribe(_ =>
        {
            switch (CurrentState)
            {
                case ActionButtonState.FishingBattle:
                    _onReleaseFishingBattleButton.OnNext(true);
                    break;
            }
        });
    }
}
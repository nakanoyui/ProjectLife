using UniRx;
using UnityEngine;

public class ItemSelecter : MonoBehaviour
{
    public enum ItemState
    {
        None,
        Fishing,
        Farming,
        Max
    }

    [SerializeField] private ActionButton _actionButton;

    [SerializeField] private ItemPresets _itemPresets;

    public ItemState _currentItemState;

    private readonly Subject<ItemState> _onChangeItemState = new();

    private Item _currentItem;

    private void Start()
    {
        OnEvent();

        _onChangeItemState.OnNext(ItemState.None);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _onChangeItemState.OnNext(_currentItemState + 1 == ItemState.Max ? ItemState.None : _currentItemState + 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            _onChangeItemState.OnNext(
                _currentItemState - 1 < ItemState.None ? ItemState.Max - 1 : _currentItemState - 1);
    }

    private void OnEvent()
    {
        _onChangeItemState.Subscribe(state =>
        {
            _currentItemState = state;

            switch (_currentItemState)
            {
                case ItemState.None:
                    _itemPresets.ChangeItem(ref _currentItem, (int)ItemState.None);
                    _actionButton.CurrentState = ActionButton.ActionButtonState.None;
                    break;
                case ItemState.Fishing:
                    _itemPresets.ChangeItem(ref _currentItem, (int)ItemState.Fishing);
                    _actionButton.CurrentState = ActionButton.ActionButtonState.Cast;
                    break;
                case ItemState.Farming:
                    _itemPresets.ChangeItem(ref _currentItem, (int)ItemState.Farming);
                    _actionButton.CurrentState = ActionButton.ActionButtonState.PlantsWatering;
                    break;
            }
        });
    }
}
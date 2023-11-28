using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class FishBrain : MonoBehaviour
{
    [SerializeField] private float _restTime;

    [SerializeField] private float _getOutOfLineTime;

    [SerializeField] private float _attractSpeed;

    [SerializeField] private float _separateSpeed;

    [SerializeField] private float biteSpeed;

    private readonly Subject<bool> _onStateChanger = new();

    private readonly CancellationToken _token = new();

    private ActionButton _actionButton;

    private FishState _fishState;

    private LureController _lureController;

    private Renderer _ownRenderer;
    private Transform _playerTransform;

    public Subject<Vector3> OnCast { get; } = new();

    private void Start()
    {
        _ownRenderer = GetComponentInChildren<Renderer>();

        OnEvent();
    }

    public void Init(Transform playerTransform, LureController lureController, ActionButton actionButton)
    {
        _playerTransform = playerTransform;
        _lureController = lureController;
        _actionButton = actionButton;
    }

    private void OnEvent()
    {
        OnCast.Subscribe(targetPos => { LureBiteAsync(targetPos); });

        _onStateChanger.Subscribe(_ => { FishStateChangeAsync().Forget(); });

        _actionButton.OnHoldFishingBattleButton.Subscribe(_ =>
        {
            if (_lureController.FishBrain != this) return;

            var targetPos = _playerTransform.position;
            targetPos.y = 0.0f;
            var ownPos = transform.position;
            ownPos.y = 0.0f;
            var dir = (targetPos - ownPos).normalized;

            transform.rotation = Quaternion.LookRotation(dir);

            switch (_fishState)
            {
                case FishState.Rest:
                    transform.position += dir * Time.deltaTime * _attractSpeed;
                    _lureController.transform.position += dir * Time.deltaTime * _attractSpeed;

                    var distance = _playerTransform.position - _lureController.transform.position;

                    // 釣りあげた
                    if (distance.sqrMagnitude < 3.0f)
                    {
                        _ownRenderer.material.color = Color.white;
                        _lureController.GetComponent<Renderer>().material.color = Color.white;
                        _lureController.OnCallBack.Invoke();
                        _actionButton.CurrentState = ActionButton.ActionButtonState.None;
                        Destroy(gameObject);
                    }

                    break;
                case FishState.GetOutOfLine:
                    transform.position -= dir * Time.deltaTime * _separateSpeed;
                    _lureController.transform.position -= dir * Time.deltaTime * _separateSpeed;
                    break;
            }
        });

        _actionButton.OnReleaseFishingBattleButton.Subscribe(_ =>
        {
            if (_lureController.FishBrain != this) return;

            var dir = (_playerTransform.position - transform.position).normalized;

            switch (_fishState)
            {
                case FishState.Rest:
                    // Nothing
                    break;
                case FishState.GetOutOfLine:
                    // Nothing
                    break;
            }
        });
    }

    private async void LureBiteAsync(Vector3 targetPos)
    {
        var dir = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(dir);

        await transform.DOMove(targetPos, biteSpeed).SetEase(Ease.OutCubic).AsyncWaitForCompletion();

        _actionButton.CurrentState = ActionButton.ActionButtonState.FishingBattle;

        _onStateChanger.OnNext(true);

        _lureController.GetComponent<Renderer>().material.color = Color.gray;
    }

    private async UniTask FishStateChangeAsync()
    {
        _token.ThrowIfCancellationRequested();

        var _time = 0.0f;

        while (true)
        {
            if (_actionButton.CurrentState != ActionButton.ActionButtonState.FishingBattle) break;

            _time += Time.deltaTime;

            switch (_fishState)
            {
                case FishState.Rest:
                    _ownRenderer.material.color = Color.blue;
                    if (_time > _restTime)
                    {
                        _time = 0.0f;
                        _fishState = FishState.GetOutOfLine;
                    }

                    break;
                case FishState.GetOutOfLine:
                    _ownRenderer.material.color = Color.red;
                    if (_time > _getOutOfLineTime)
                    {
                        _time = 0.0f;
                        _fishState = FishState.Rest;
                    }

                    break;
            }

            await UniTask.Yield(_token);
        }
    }

    private enum FishState
    {
        Rest,
        GetOutOfLine
    }
}
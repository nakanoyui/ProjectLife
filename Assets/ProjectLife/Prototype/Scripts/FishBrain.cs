using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class FishBrain : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private LureController _lureController;
    
    [SerializeField] private ActionButton _actionButton;

    private Renderer _ownRenderer;
    
    private Subject<Vector3> _onCast = new();

    public Subject<Vector3> OnCast
    {
        get { return _onCast; }
    }

    private Subject<bool> _onStateChanger = new();

    [SerializeField] private float _restTime;
    
    [SerializeField] private float _getOutOfLineTime;

    [SerializeField] private float _attractSpeed;
    
    [SerializeField] private float _separateSpeed;

    [SerializeField] private float biteSpeed;
    
    private enum FishState
    {
        Rest,
        GetOutOfLine,
    }

    private FishState _fishState;
    
    private void Start()
    {
        _ownRenderer = GetComponentInChildren<Renderer>();
        
        OnEvent();
    }

    private void OnEvent()
    {
        _onCast.Subscribe(targetPos => { LureBite(targetPos); });

        _onStateChanger.Subscribe(_ => { StartCoroutine(FishStateChange());});
        
        _actionButton.OnHoldFishingBattleButton.Subscribe(_ =>
        {
            var targetPos = _playerTransform.position;
            targetPos.y = 0.0f;
            var ownPos = transform.position;
            ownPos.y = 0.0f;
            var dir =(targetPos - ownPos).normalized;

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

    private async void LureBite(Vector3 targetPos)
    {
        var dir = (targetPos - transform.position).normalized;
        
        transform.rotation = Quaternion.LookRotation(dir);
        
        await transform.DOMove(targetPos, biteSpeed).SetEase(Ease.OutCubic).AsyncWaitForCompletion();

        // Change Fishing Battle
        _actionButton.CurrentState = ActionButton.ActionButtonState.FishingBattle;

        _onStateChanger.OnNext(true);
        
        _lureController.GetComponent<Renderer>().material.color = Color.gray;
    }

    private IEnumerator FishStateChange()
    {
        float _time = 0.0f;
        
        while (true)
        {
            if (_actionButton.CurrentState != ActionButton.ActionButtonState.FishingBattle)
            {
                yield break;
            }

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
            
            yield return null;
        }
    }
}
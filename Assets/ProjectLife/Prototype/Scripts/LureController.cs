using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// LureController
/// </summary>
public class LureController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private ActionButton _actionButton;

    private FishBrain _fishBrain;
    
    public FishBrain FishBrain
    {
        get { return _fishBrain;}
        set { _fishBrain = value; }
    }

    private UnityEvent _onCallBack = new UnityEvent();
    
    public UnityEvent OnCallBack
    {
        get { return _onCallBack; }
    }
    
    [SerializeField, Range(0.0f, 5.0f)] private float _castDistance;
    
    [SerializeField, Range(0.0f, 5.0f)] private float _castHeight;
    
    [SerializeField, Range(0.0f, 5.0f)] private float _backHeight;

    private Vector3 _originPosition;
    
    [SerializeField] private float _castSpeed = 1.0f;
    [SerializeField] private float _backSpeed = 1.5f;
    
    private CancellationToken _token = new();
    
    private void Start()
    {
        _onCallBack.AddListener(Back);
        
        OnEvent();
    }

    private void OnEvent()
    {
        _actionButton.OnClickCastButton.Subscribe(_ => { Cast(); });

        _actionButton.OnClickFishButton.Subscribe(_ => { _onCallBack.Invoke(); });
    }

    private void Cast()
    {
        InputBlocker.Enable();
        
        _originPosition = transform.position;
        
        var startPos = transform.position;
        
        var forward = _playerTransform.forward;

        var endPos = startPos + forward * _castDistance;
        
        // Todo:将来的に海の高さに変更する
        endPos.y = 0;
        
        Vector3 half = Vector3.Lerp(startPos, endPos, 0.75f);
        half.y += Vector3.up.y + _castHeight;
        
        LerpCastAsync(transform,startPos,half, endPos, _castSpeed).Forget();
    }

    private void Back()
    {
        InputBlocker.Enable();

        var startPos = transform.position;

        var endPos = _originPosition;
        
        Vector3 half = Vector3.Lerp(startPos, endPos, 0.75f);
        half.y += Vector3.up.y + _backHeight;
        
        LerpBackAsync(transform,startPos,half,endPos,_backSpeed).Forget();
    }

    private async UniTask LerpCastAsync(Transform target, Vector3 start,Vector3 half, Vector3 end, float duration)
    {
        _token.ThrowIfCancellationRequested();
        
        float speed = 0f;
        while (true)
        {
            if (speed >= 1.0f)
            {
                _fishBrain.OnCast.OnNext(target.position);
                InputBlocker.Disable();
                
                break;
            }

            speed += Time.deltaTime / duration;
            target.position = CalcLerpPoint(start, half,end, speed);

            await UniTask.Yield(_token);
        }
    }

    private async UniTask LerpBackAsync(Transform target, Vector3 start,Vector3 half, Vector3 end, float duration)
    {
        _token.ThrowIfCancellationRequested();
        
        float speed = 0f;
        while (true)
        {
            if (speed >= 1.0f)
            {
                _actionButton.CurrentState = ActionButton.ActionButtonState.Cast;
                InputBlocker.Disable();

                break;
            }

            speed += Time.deltaTime / duration;
            target.position = CalcLerpPoint(start, half,end, speed);

            await UniTask.Yield(_token);
        }
    }

    Vector3 CalcLerpPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t);
        var b = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(a, b, t);
    }
}
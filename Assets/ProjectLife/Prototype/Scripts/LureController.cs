using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.Serialization;

/// <summary>
/// Lurewe]]e7Controller
/// </summary>
public class LureController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private ActionButton _actionButton;

    [SerializeField] private FishBrain _fishBrain;
    
    [SerializeField, Range(0.0f, 5.0f)] private float _castHeight;
    
    [SerializeField, Range(0.0f, 5.0f)] private float _backHeight;

    private Vector3 _originPosition;
    
    [SerializeField] private float _castSpeed = 1.0f;
    [SerializeField] private float _backSpeed = 1.5f;
    
    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        _actionButton.OnClickCastButton.Subscribe(holdPower => { Cast(holdPower); });

        _actionButton.OnClickFishButton.Subscribe(_ => { Back(); });
    }

    private void Cast(float holdPower)
    {
        _originPosition = transform.position;
        
        var startPos = transform.position;
        
        var forward = _playerController.transform.forward;

        var endPos = startPos + forward * holdPower;
        
        // Todo:将来的に海の高さに変更する
        endPos.y = 0;
        
        Vector3 half = Vector3.Lerp(startPos, endPos, 0.75f);
        half.y += Vector3.up.y + _castHeight;
            
        StartCoroutine(LerpCast(transform,startPos,half, endPos, _castSpeed));
    }

    private void Back()
    {
        var startPos = transform.position;

        var endPos = _originPosition;
        
        Vector3 half = Vector3.Lerp(startPos, endPos, 0.75f);
        half.y += Vector3.up.y + _backHeight;
        
        StartCoroutine(LerpBack(transform,startPos,half,endPos,_backSpeed));
    }

    private IEnumerator LerpCast(Transform target, Vector3 start,Vector3 half, Vector3 end, float duration)
    {
        float speed = 0f;
        while (true)
        {
            if (speed >= 1.0f)
            {
                _fishBrain.OnCast.OnNext(target.position);
                yield break;
            }

            speed += Time.deltaTime / duration;
            target.position = CalcLerpPoint(start, half,end, speed);

            yield return null;
        }
    }

    private IEnumerator LerpBack(Transform target, Vector3 start,Vector3 half, Vector3 end, float duration)
    {
        float speed = 0f;
        while (true)
        {
            if (speed >= 1.0f)
            {
                _actionButton.CurrentState = ActionButton.ActionButtonState.Cast;
                yield break;
            }

            speed += Time.deltaTime / duration;
            target.position = CalcLerpPoint(start, half,end, speed);

            yield return null;
        }
    }

    Vector3 CalcLerpPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t);
        var b = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(a, b, t);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.Serialization;

/// <summary>
/// FishController
/// </summary>
public class FishController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private ActionButton _actionButton;

    [SerializeField, Range(0.0f, 5.0f)] private float _castHeight;
    
    private Transform _targetTransform;

    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        _actionButton.OnClickCastButton.Subscribe(holdPower => { Cast(holdPower); });

        _actionButton.OnClickFishButton.Subscribe(_ => { });
    }

    private void Cast(float holdPower)
    {
        var startPos = transform.position;
        
        var forward = _playerController.transform.forward;

        var endPos = transform.position + forward * holdPower;
        // Todo:将来的に海の高さに変更する
        endPos.y = 0;
        
        Vector3 half = Vector3.Lerp(startPos, endPos, 0.75f);
        half.y += Vector3.up.y + _castHeight;
            
        StartCoroutine(LerpThrow(gameObject,startPos,half, endPos, 1.0f));
    }

    private IEnumerator LerpThrow(GameObject target, Vector3 start,Vector3 half, Vector3 end, float duration)
    {
        float speed = 0f;
        while (true)
        {
            if (speed >= 1.0f)
                yield break;

            speed += Time.deltaTime / duration;
            target.transform.position = CalcLerpPoint(start, half,end, speed);

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
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class FishBrain : MonoBehaviour
{
    private Subject<Vector3> _onCast = new();

    public Subject<Vector3> OnCast
    {
        get { return _onCast; }
    }

    [SerializeField] private float biteSpeed = 3.0f;

    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        _onCast.Subscribe(targetPos => { LureBite(targetPos); });
    }

    private void LureBite(Vector3 targetPos)
    {
        var dir = (targetPos - transform.position).normalized;
        
        transform.rotation = Quaternion.LookRotation(dir);
        
        transform.DOMove(targetPos, biteSpeed).SetEase(Ease.OutCubic);
    }
}
using System;
using UniRx;
using UnityEngine;

/// <summary>
/// Button
/// </summary>
public class Button : MonoBehaviour
{
    private Subject<bool> _onButtonDown = new();
    private Subject<bool> _onButtonUp = new();
    private Subject<bool> _onButtonHold = new();
    
    public IObservable<bool> OnButtonDown { get { return _onButtonDown; } }
    public IObservable<bool> OnButtonUp { get { return _onButtonUp; } }
    public IObservable<bool> OnButtonHold { get { return _onButtonHold; } }

    private bool _isHold;

    private void Update()
    {
        if (_isHold)
        {
            _onButtonHold.OnNext(true);
        }
    }

    public void OnPointerDown()
    {
        _onButtonDown.OnNext(true);
        _isHold = true;
    }
    
    public void OnPointerUp()
    {
        _onButtonUp.OnNext(true);
        _isHold = false;
    }
}
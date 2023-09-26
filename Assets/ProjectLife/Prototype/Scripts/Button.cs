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
    private Subject<bool> _onButtonRelease = new();
    
    public IObservable<bool> OnButtonDown { get { return _onButtonDown; } }
    public IObservable<bool> OnButtonUp { get { return _onButtonUp; } }
    public IObservable<bool> OnButtonHold { get { return _onButtonHold; } }
    
    public IObservable<bool> OnButtonRelease { get { return _onButtonRelease; } }

    private bool _isHold;

    private bool _isEnter;

    private void Update()
    {
        if (_isHold)
        {
            _onButtonHold.OnNext(true);
        }

        if (!Input.GetMouseButtonDown(0) || !_isEnter && Input.GetMouseButtonDown(0))
        {
            _onButtonRelease.OnNext(true);
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

    public void OnPointerEnter()
    {
        _isEnter = true;
    }

    public void OnPointerExit()
    {
        _isEnter = false;
    }
}
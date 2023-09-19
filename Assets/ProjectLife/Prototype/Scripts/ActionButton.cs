using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// ActionButton
/// </summary>
public class ActionButton : MonoBehaviour
{
    private Subject<bool> _onClickButton = new ();

    public IObservable<bool> OnClickButton
    {
        get { return _onClickButton; }
    }
    
    public void OnClick()
    {
        _onClickButton.OnNext(true);
    }

    public enum StateType
    {
        None,
        Fishing,
        
    }
}

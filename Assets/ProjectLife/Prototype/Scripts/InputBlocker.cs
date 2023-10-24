using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    private static Image _blockImage;

    private static InputBlocker _inputBlocker = null;
    
    private void Start()
    {
        _blockImage = _image;
        _blockImage.raycastTarget = false;
    }

    // 入力阻止を有効にします。
    public static void Enable()
    {
        _blockImage.raycastTarget = true;
    }
    
    // 入力阻止を無効にする
    public static void Disable()
    {
        _blockImage.raycastTarget = false;
    }
}

// 入力阻止を紐づけるオブジェクト
// usingステートメントはIDisableを継承したオブジェクトを渡す事が出来る
public class InputBlockObject : IDisposable
{
    public InputBlockObject()
    {
        InputBlocker.Enable();
    }

    public void Dispose()
    {
        InputBlocker.Disable();
    }
}
